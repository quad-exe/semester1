using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Animator animator;

    [Header("Ghost Movement")]
    public float ghostSpeed = 6f;
    public float ghostDuration = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("Respawn")]
    public Transform RespawnPoint;

    [Header("Ghost Settings")]
    public GameObject ghostObject;

    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private bool isSwapping = false;
    private Animator ghostAnimator;
    private bool isGhostActive = false;

    private Rigidbody2D ghostRb;
    private SpriteRenderer ghostRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 3f;
        animator = GetComponent<Animator>();
       

        if (ghostObject != null)
        {
            ghostObject.SetActive(false);
            ghostAnimator = ghostObject.GetComponent<Animator>();
            ghostRenderer = ghostObject.GetComponent<SpriteRenderer>();

            ghostRb = ghostObject.GetComponent<Rigidbody2D>();
            if (ghostRb == null)
            {
                ghostRb = ghostObject.AddComponent<Rigidbody2D>();
                ghostRb.gravityScale = 0f;
                ghostRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

   void Update()
{
    float moveInput = 0f;

    if (!isGhostActive)
    {
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (moveInput != 0)
            spriteRenderer.flipX = moveInput < 0;

        // Update walking animation
        SetAnimation(moveInput);

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer | playerLayer);

        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isSwapping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(SwapToGhost());
        }
    }
        else
        {
            // --- Ghost flying controls ---
            float h = 0f, v = 0f;
            if (Input.GetKey(KeyCode.LeftArrow)) h = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) h = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) v = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) v = -1f;

            Vector2 move = new Vector2(h, v).normalized;
            ghostRb.linearVelocity = move * ghostSpeed;

            // --- Flip ghost sprite direction ---
            if (h != 0 && ghostRenderer != null)
                ghostRenderer.flipX = h < 0;
        }
    }
    private void SetAnimation(float moveInput)
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;
        animator.SetBool("IsMoving", isMoving);
    }

    private IEnumerator SwapToGhost()
    {
        isSwapping = true;
        isGhostActive = true;

        // Hide player
        spriteRenderer.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        // Activate ghost
        if (ghostObject != null)
        {
            ghostObject.SetActive(true);
            ghostObject.transform.position = transform.position;

            if (ghostAnimator != null)
            {
                ghostAnimator.Rebind();
                ghostAnimator.Update(0f);
                ghostAnimator.SetTrigger("Play");
            }
        }

        yield return new WaitForSeconds(ghostDuration);

        // Return to player at ghost's position
        if (ghostObject != null)
        {
            transform.position = ghostObject.transform.position;
            ghostObject.SetActive(false);
        }

        rb.gravityScale = 3f;
        spriteRenderer.enabled = true;
        isGhostActive = false;
        isSwapping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes") || collision.CompareTag("LavaPit"))
        {
            if (RespawnPoint == null)
            {
                Debug.LogError("RespawnPoint not assigned!");
                return;
            }

            transform.position = new Vector3(
                RespawnPoint.position.x,
                RespawnPoint.position.y,
                transform.position.z
            );

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}


   

