using UnityEngine;

public class PlayerTwo : MonoBehaviour
{
    private Rigidbody2D Rb;
    float Verti;
    float Hori;
    public float MoveSpeed;
    public Transform RespawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.A))   // hold to move left
        {
            move += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))   // hold to move right
        {
            move += Vector3.right;
        }

        transform.position += move * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            if (RespawnPoint == null)
            {
                Debug.LogError("RespawnPoint not assigned!");
                return;
            }

            // Reset position (keep original Z so player doesn’t vanish from camera)
            transform.position = new Vector3(
                RespawnPoint.position.x,
                RespawnPoint.position.y,
                transform.position.z
            );

            // Reset physics
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}

