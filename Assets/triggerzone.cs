using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    public MovingPlatform targetPlatform;  // The platform to activate
    public KeyCode activateKey = KeyCode.LeftShift; // Which key activates it

    private bool playerInside = false;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(activateKey))
        {
            if (targetPlatform != null)
            {
                targetPlatform.Activate();
                Debug.Log("Button pressed! Platform activated.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player on button. Press " + activateKey + " to activate.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player left the button.");
        }
    }
}

