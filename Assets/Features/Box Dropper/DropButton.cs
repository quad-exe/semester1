using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxSpawnerButton : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject boxPrefab;
    public Transform spawnPoint;

    [Header("Button Settings")]
    public Animator buttonAnimator;
    public string pressTrigger = "Press";

    [Header("Box Limit")]
    public int maxBoxes = 4;

    [Header("Box Physics")]
    public float gravityScale = 1f;
    public float dropSpeed = 1f;

    private bool playerInside = false;
    private bool isAnimating = false; // Prevent spamming
    private List<GameObject> spawnedBoxes = new List<GameObject>();

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.RightShift) && !isAnimating)
        {
            StartCoroutine(PressButtonRoutine());
        }
    }

    private IEnumerator PressButtonRoutine()
    {
        isAnimating = true;

        // --- Play button animation ---
        if (buttonAnimator != null && !string.IsNullOrEmpty(pressTrigger))
        {
            buttonAnimator.SetTrigger(pressTrigger);
        }

        // Optional: wait a small delay to sync box spawn with animation
        yield return new WaitForSeconds(0.1f);

        // --- Spawn box ---
        if (boxPrefab != null && spawnPoint != null)
        {
            GameObject newBox = Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);

            Rigidbody2D rb = newBox.GetComponent<Rigidbody2D>();
            if (rb != null)
            rb.gravityScale = gravityScale;
            rb.linearVelocity = Vector2.down * dropSpeed;


            spawnedBoxes.Add(newBox);

            // --- Limit boxes to maxBoxes ---
            if (spawnedBoxes.Count > maxBoxes)
            {
                Destroy(spawnedBoxes[0]);
                spawnedBoxes.RemoveAt(0);
            }
        }

        // Wait for the animation to finish
        if (buttonAnimator != null)
        {
            // Get animation clip length
            AnimatorClipInfo[] clips = buttonAnimator.GetCurrentAnimatorClipInfo(0);
            float animLength = clips.Length > 0 ? clips[0].clip.length : 0.5f;
            yield return new WaitForSeconds(animLength);
        }
        else
        {
            // Default fallback
            yield return new WaitForSeconds(0.5f);
        }

        isAnimating = false; // Ready for next press
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Square"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Square"))
            playerInside = false;
    }
}


