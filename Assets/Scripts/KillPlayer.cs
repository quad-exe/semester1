using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    // KillPlayers
    public GameObject player1;
    public GameObject player2;
    public Transform respawnPoint1;
    public Transform respawnPoint2;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            player1.transform.position = respawnPoint1.position;
        }
        if (other.gameObject.CompareTag("Player2"))
        {
            player2.transform.position = respawnPoint2.position;
        }
    }

}
