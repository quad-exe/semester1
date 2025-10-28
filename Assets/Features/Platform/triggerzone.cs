using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    [Header("Activation Settings")]
    public string targetPlatformTag = "PlatformA"; // Tag of the platform to activate
    public KeyCode activateKey = KeyCode.LeftShift; // Which key to press

    private bool playerInside = false;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(activateKey))
        {
            // Find all MovingPlatforms in the scene
            GameObject[] platforms = GameObject.FindGameObjectsWithTag(targetPlatformTag);

            foreach (GameObject platformObj in platforms)
            {
                MovingPlatform platform = platformObj.GetComponent<MovingPlatform>();
                if (platform != null)
                {
                    platform.Activate();
                    Debug.Log($"Activated platform with tag {targetPlatformTag}");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            playerInside = true;
            Debug.Log($"Player entered button zone. Press {activateKey} to activate {targetPlatformTag}.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            playerInside = false;
            Debug.Log("Player left the button zone.");
        }
    }
}

