using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class HorizontalCatapult : MonoBehaviour
{
    [Header("Catapult Settings")]
    public float chargeDistance = 1f;       // how far platform moves left when charging
    public float chargeSpeed = 2f;          // slow charge speed
    public float releaseDistance = 1f;      // how far platform moves right when releasing
    public float releaseSpeed = 10f;        // fast release speed
    public float launchForce = 20f;         // horizontal force applied to boxes
    public KeyCode launchKey = KeyCode.E;   // interaction key
    public string playerTag = "Player1";    // player detection tag
    public string boxTag = "Box";           // box detection tag

    private Rigidbody2D rb;
    private Vector2 startPosition;

    private bool playerInRange = false;
    private bool isLaunching = false;

    // Boxes currently sitting on the platform
    private readonly List<Rigidbody2D> boxesOnPlatform = new List<Rigidbody2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = rb.position;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(launchKey) && !isLaunching)
        {
            StartCoroutine(ChargeAndRelease());
        }
    }

    private IEnumerator ChargeAndRelease()
    {
        isLaunching = true;

        // 1️⃣ Charge phase: move left slowly
        Vector2 chargeTarget = startPosition + Vector2.left * chargeDistance;
        while (Vector2.Distance(rb.position, chargeTarget) > 0.01f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, chargeTarget, chargeSpeed * Time.deltaTime));
            yield return null;
        }

        // 2️⃣ Release phase: move right fast by a fixed distance
        Vector2 releaseTarget = chargeTarget + Vector2.right * releaseDistance;
        bool boxesLaunched = false;
        while (Vector2.Distance(rb.position, releaseTarget) > 0.01f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, releaseTarget, releaseSpeed * Time.deltaTime));

            // Launch boxes at the start of release
            if (!boxesLaunched && boxesOnPlatform.Count > 0)
            {
                foreach (var box in boxesOnPlatform)
                {
                    if (box != null)
                        box.AddForce(Vector2.right * launchForce, ForceMode2D.Impulse);
                }
                boxesOnPlatform.Clear();
                boxesLaunched = true;
            }

            yield return null;
        }

        // 3️⃣ Return to start position
        while (Vector2.Distance(rb.position, startPosition) > 0.01f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, startPosition, releaseSpeed * Time.deltaTime));
            yield return null;
        }

        isLaunching = false;
    }

    // Detect player in trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            Debug.Log("Player entered trigger zone: " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            Debug.Log("Player left trigger zone: " + other.name);
        }
    }

    // Detect boxes sitting on the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(boxTag))
        {
            Rigidbody2D boxRb = collision.rigidbody;
            if (boxRb != null && !boxesOnPlatform.Contains(boxRb))
            {
                boxesOnPlatform.Add(boxRb);
                Debug.Log("Box on platform: " + boxRb.name);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(boxTag))
        {
            Rigidbody2D boxRb = collision.rigidbody;
            if (boxRb != null && boxesOnPlatform.Contains(boxRb))
            {
                boxesOnPlatform.Remove(boxRb);
                Debug.Log("Box left platform: " + boxRb.name);
            }
        }
    }
}
