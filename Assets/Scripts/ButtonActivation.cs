using UnityEngine;

public class SwitchActivation : MonoBehaviour
{
    [Header("Switch Settings")]
    public KeyCode activationKey = KeyCode.E; // Key to activate the switch
    public bool isActivated = false;          // True when activated

    private bool playerInRange = false;       // Is any player standing on the switch?

    private void Update()
    {
        // Only allow activation if a player is in range and presses the key
        if (playerInRange && Input.GetKeyDown(activationKey))
        {
            isActivated = true;
            Debug.Log("Switch activated!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to either player
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = true;
            Debug.Log(other.name + " is on the switch.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Player leaves switch
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = false;
            Debug.Log(other.name + " left the switch.");
        }
    }

    // Optional: manually reset the switch
    public void ResetSwitch()
    {
        isActivated = false;
        Debug.Log("Switch reset.");
    }
}
