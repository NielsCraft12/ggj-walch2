using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles player movement, cleaning mechanics, and trap placement
public class playerMovement : MonoBehaviour
{
    // Component reference
    private Rigidbody rb;

    [Header("Movement")]
    // Movement settings
    [SerializeField]
    private float maxSpeed; // movement speed

    [SerializeField]
    private float rotationSpeed = 5f; // Speed at which player rotates
    private Vector2 moveDirection; // Current movement input direction

    [Header("Cleaning")]
    // Cleaning mechanic variables
    [SerializeField]
    GameObject cleaningpoint; // Point from which cleaning occurs
    private float cleaningTimer; // Timer for cleaning duration

    [SerializeField]
    private float cleaningTime = 2f; // How long cleaning takes

    [SerializeField]
    private float cleaningRadius = 2f; // Area of effect for cleaning
    bool isCleaning = false; // Cleaning state flag

    [Header("Particles")]
    // Particle effect handling
    [SerializeField]
    GameObject particles; // Particle system for visual effects
    bool isCleard = false; // Flag to track particle system state

    [SerializeField]
    bool particlesActive = false; // Toggle for particle effects

    [Header("Traps")]
    // Trap placement system
    [SerializeField]
    GameObject trap; // Trap prefab to spawn

    [SerializeField]
    Transform trapSpawnPoint; // Where traps spawn from

    [SerializeField]
    float trapcooldown = 1f; // Time between trap placements

    [SerializeField]
    float trapcooldownTimer = 0f; // Current cooldown timer

    GameManager manager;

    // Initialize physics properties
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0.5f; // Remove air resistance
        rb.mass = 1f; // Set consistent mass
        rb.angularDamping = 0.05f; // Minimal rotation resistance
        rb.useGravity = true; // Ensure gravity is on
    }

    private void Start()
    {
        manager = FindFirstObjectByType<GameManager>();
    }

    // Handle timers and visual effects
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

    // Physics-based movement calculations
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

    // Toggle pause state
    public void Pause(InputAction.CallbackContext _context)
    {
        SettingsSingleton.Instance.settings.m_IsPaused = !SettingsSingleton
            .Instance
            .settings
            .m_IsPaused;
    }

    // Handle trap placement with cooldown
    public void ShootTrap(InputAction.CallbackContext _context)
    {
        if (trapcooldownTimer <= 0)
        {
            AudioManager.m_Instance.Play("BigPop");
            Instantiate(trap, trapSpawnPoint.position, trapSpawnPoint.rotation);
            trapcooldownTimer = trapcooldown;
        }
    }

    // Process movement input
    public void OnMove(InputAction.CallbackContext _context)
    {
        moveDirection = _context.ReadValue<Vector2>();
    }

    // Handle cleaning action input
    public void Clean(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Performed)
        {
            AudioManager.m_Instance.Play("Bubble");
            cleaningTimer = 2f;
            isCleaning = true;
        }
        else if (_context.phase == InputActionPhase.Canceled)
        {
            AudioManager.m_Instance.Stop("Bubble");
            isCleaning = false;
            particlesActive = false;
        }
    }

    // Rotate player to face movement direction
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

    // Handle door interactions
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

    // Visualize cleaning radius in editor
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
            if (hitCollider.CompareTag("Trash") || hitCollider.CompareTag("Rat"))
            {
                if (cleaningTimer < 0)
                {
                    AudioManager.m_Instance.Play("Slerp");
                    manager.m_CleanedCount++;
                    Destroy(hitCollider.gameObject);
                    isCleaning = false;
                    particlesActive = false;
                }
            }
        }
    }
}
