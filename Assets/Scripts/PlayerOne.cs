using Unity.Hierarchy;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Respawn")]
    public Transform RespawnPoint;
    // No need to assign here � will find GameController automatically


    private Rigidbody2D rb;
    private bool canJump = true;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
    }

    void Update()
    {
        // --- Horizontal movement ---
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // --- Ground check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
            canJump = true;

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.UpArrow) && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset vertical velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
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
        void OnDrawGizmosSelected()
        {

            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
       
            
   

