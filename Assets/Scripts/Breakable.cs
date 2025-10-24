using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Break Settings")]
    public GameObject breakEffect;     // Optional particle/debris prefab
    public AudioClip breakSound;
    public float destroyDelay = 3f;    // How long before it's fully gone
    public float breakForce = 3f;      // Small random force when it breaks

    private bool isBroken = false;

    public void Break()
    {
        if (isBroken) return;
        isBroken = true;

        // Play sound
        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

        // Spawn debris / particle effect
        if (breakEffect != null)
            Instantiate(breakEffect, transform.position, Quaternion.identity);

        // --- Make the wall fall ---
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Enable gravity & random tumble
        rb.gravityScale = 2f;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), 1f) * breakForce, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);

        // Disable collisions after a short moment (so it doesn’t block stuff)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Destroy object after delay
        Destroy(gameObject, destroyDelay);
    }
}
