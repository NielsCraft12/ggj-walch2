using UnityEngine;

public class Rattrap : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 15.0f; // Increased default speed for arrow-like behavior

    [SerializeField]
    GameObject rat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, 5);
    }

    void Update()
    {
        // Make the arrow rotate to match its velocity direction
        if (rb.linearVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Rat"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rat"))
        {
            Destroy(other.gameObject);
        }

        if (!other.gameObject.CompareTag("Player") || !other.gameObject.CompareTag("Scientist"))
        {
            Destroy(gameObject);
        }
    }
}
