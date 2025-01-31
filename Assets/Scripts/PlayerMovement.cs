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

    [SerializeField]
    GameObject particles;

    bool isCleard = false;

    [SerializeField]
    bool particlesActive = false;

    [SerializeField]
    GameObject trap;

    [SerializeField]
    Transform trapSpawnPoint;

    [SerializeField]
    float trapcooldown = 1f;

    [SerializeField]
    float trapcooldownTimer = 0f;

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
        if (trapcooldownTimer > 0)
        {
            trapcooldownTimer -= Time.deltaTime;
        }

        if (particlesActive == true)
        {
            if (!isCleard)
            {
                particles.GetComponent<ParticleSystem>().Simulate(0, true, true);
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

    public void ShootTrap(InputAction.CallbackContext _context)
    {
        if (trapcooldownTimer <= 0)
        {
            Instantiate(trap, trapSpawnPoint.position, trapSpawnPoint.rotation);
            trapcooldownTimer = trapcooldown;
        }
    }

    public void Pause(InputAction.CallbackContext _context)
    {
        SettingsSingleton.Instance.settings.m_IsPaused = !SettingsSingleton
            .Instance
            .settings
            .m_IsPaused;
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
        }
        else if (_context.phase == InputActionPhase.Canceled)
        {
            isCleaning = false;
            particlesActive = false;
        }
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
