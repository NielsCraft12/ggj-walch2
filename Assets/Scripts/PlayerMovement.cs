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

    [SerializeField]
    private float cleaningRadius = 2f; // Radius for the cleaning sphere

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
        LookAt();
        // CheckCleaningArea();
    }

    private void FixedUpdate()
    {
        // Maintain the vertical velocity to avoid interference with jumping
        float yVelocity = rb.linearVelocity.y;

        // Apply the calculated movement direction and speed
        rb.linearVelocity = new Vector3(
            moveDirection.x * maxSpeed,
            yVelocity,
            moveDirection.y * maxSpeed
        );
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        moveDirection = _context.ReadValue<Vector2>();
    }

    public void Clean(InputAction.CallbackContext _context) { }

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
                Debug.Log($"Object in cleaning range: {hitCollider.gameObject.name}");
                Destroy(hitCollider.gameObject);
            }
        }
    }
}
