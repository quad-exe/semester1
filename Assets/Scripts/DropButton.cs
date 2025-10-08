using UnityEngine;

public class BoxSpawnerButton : MonoBehaviour
{
    public GameObject boxPrefab;      // the box prefab
    public Transform spawnPoint;      // where the box should appear
    public KeyCode activateKey = KeyCode.E;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(activateKey))
        {
            if (boxPrefab != null && spawnPoint != null)
            {
                // Instantiate a new box at spawnPoint
                GameObject newBox = Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);

                // Ensure the Rigidbody2D is active and gravity is on
                Rigidbody2D rb = newBox.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.gravityScale = 1f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player2"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player2"))
            playerInside = false;
    }
}

