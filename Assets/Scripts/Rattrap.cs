using UnityEngine;

public class Rattrap : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 1.0f;

    [SerializeField]
    GameObject rat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rat.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update() { }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Rat")
        {
            rat.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}
