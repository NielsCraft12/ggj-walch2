using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float airControlMultiplier = 0.5f; // Add this field - controls air movement strength

    private Vector2 moveDirection;
    public Vector2 lastMoveDirection;
    private bool isGrounded = true;

    //  private Animator animator;

    // private GameManager gameManager;

    private bool isKnockedBack = false; // Add this field

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0.5f; // Remove air resistance
        rb.mass = 1f; // Set consistent mass
        rb.angularDamping = 0.05f; // Minimal rotation resistance
        rb.useGravity = true; // Ensure gravity is on

        //  animator = GetComponentInChildren<Animator>();

        //  gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        LookAt();

        if (moveSpeed < 0)
        {
            moveSpeed = 0;
        }

        // Animation
        // if (moveDirection != Vector2.zero)
        // {
        //     animator.SetTrigger("Walk");
        // }
        // else
        // {
        //     animator.SetTrigger("Idle");
        // }
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

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Floor"))
    //     {
    //         isGrounded = true;
    //     }
    // }

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

    // Add these methods to communicate with PlayerAttack
    // public void SetKnockback(bool knocked)
    // {
    //     isKnockedBack = knocked;
    // }
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
}
