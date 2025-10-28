using UnityEngine;

public class Player2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Yell Settings")]
    public KeyCode yellKey = KeyCode.W;
    public float yellRange = 5f;
    public float yellForce = 10f;
    public LayerMask breakableLayer;
    public AudioClip yellSound;
    public float yellCooldown = 3f; // 🕒 seconds before next yell
    private float nextYellTime = 0f; // internal timer
    private AudioSource audioSource;
    private CameraShake cameraShake;

    [Header("Respawn Settings")]
    public Transform respawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        HandleMovement();
        HandleYell();
    }

    void HandleMovement()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0)
            spriteRenderer.flipX = moveInput < 0;

        animator.SetBool("IsMoving", Mathf.Abs(moveInput) > 0.1f);
    }

    void HandleYell()
    {
        // prevent yell if still cooling down
        if (Time.time < nextYellTime)
            return;

        if (Input.GetKeyDown(yellKey))
        {
            // set next time we can yell
            nextYellTime = Time.time + yellCooldown;

            // play sound
            if (audioSource && yellSound)
                audioSource.PlayOneShot(yellSound);

            // camera shake
            if (cameraShake != null)
                StartCoroutine(cameraShake.Shake(0.3f, 0.2f));

            // find breakable objects
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, yellRange, breakableLayer);

            foreach (var hit in hitObjects)
            {
                Rigidbody2D hitRb = hit.attachedRigidbody;
                if (hitRb != null)
                {
                    Vector2 forceDir = (hitRb.transform.position - transform.position).normalized;
                    hitRb.AddForce(forceDir * yellForce, ForceMode2D.Impulse);
                }

                Breakable breakable = hit.GetComponent<Breakable>();
                if (breakable != null)
                    breakable.Break();
            }

            Debug.Log("YELL used! Cooldown started for " + yellCooldown + "s");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, yellRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes") || collision.CompareTag("LavaPit"))
        {
            if (respawnPoint == null)
            {
                Debug.LogError("RespawnPoint not assigned!");
                return;
            }

            transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, transform.position.z);
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            Debug.Log("Player respawned after hitting " + collision.tag);
        }
    }
}

