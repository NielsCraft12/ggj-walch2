using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float rotationSpeed = 5f;

    private Vector2 moveDirection;

    [SerializeField]
    GameObject cleaningpoint;

    private float cleaningTimer;

    [SerializeField]
    private float cleaningTime = 2f;

    [SerializeField]
    private float cleaningRadius = 2f; // Radius for the cleaning sphere

    bool isCleaning = false;

    Coroutine coroutine;

    [SerializeField]
    GameObject particles;

    bool isCleard = false;

    [SerializeField]
    bool particlesActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0.5f; // Remove air resistance
        rb.mass = 1f; // Set consistent mass
        rb.angularDamping = 0.05f; // Minimal rotation resistance
        rb.useGravity = true; // Ensure gravity is on
    }

    void Update()
    {
        if (particlesActive == true)
        {
            if (!isCleard)
            {
                particles.GetComponent<ParticleSystem>().Simulate(0, true, true);
                //  particles.GetComponent<ParticleSystem>().Clear(true);

                isCleard = true;
            }
            particles.GetComponent<ParticleSystem>().Play(true);
        }
        else
        {
            isCleard = false;
            particles.GetComponent<ParticleSystem>().Stop();
        }
        if (cleaningTimer > 0 && isCleaning == true)
        {
            particlesActive = true;
            cleaningTimer -= Time.deltaTime;
            CheckCleaningArea();
        }
        LookAt();
    }

    private void FixedUpdate()
    {
        // Maintain the vertical velocity to avoid interference with jumping
        float yVelocity = rb.linearVelocity.y;

        // Apply the calculated movement direction and speed
        rb.linearVelocity = new Vector3(
            moveDirection.y * maxSpeed,
            yVelocity,
            moveDirection.x * maxSpeed
        );
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        moveDirection = _context.ReadValue<Vector2>();
    }

    public void Clean(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Performed)
        {
            cleaningTimer = 2f;
            isCleaning = true;
            // This runs when key is first pressed
        }
        else if (_context.phase == InputActionPhase.Canceled)
        {
            isCleaning = false;
            particlesActive = false;
            Debug.Log("Clean key released");
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(cleaningTimer);
        CheckCleaningArea();
    }

    private void LookAt()
    {
        Vector3 direction = rb.linearVelocity;
        direction.y = 0f;

        if (moveDirection.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            if (other.gameObject.GetComponent<DoorOpen>().Moving == false)
            {
                other.gameObject.GetComponent<DoorOpen>().Moving = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (cleaningpoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(cleaningpoint.transform.position, cleaningRadius);
        }
    }

    private void CheckCleaningArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            cleaningpoint.transform.position,
            cleaningRadius
        );
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Trash"))
            {
                if (cleaningTimer < 0)
                {
                    Debug.Log("Cleaning complete");
                    Destroy(hitCollider.gameObject);
                }
            }
        }
    }
}
