using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    [Header("Player Setup")]
    public GameObject playerPrefab;      // assign your player prefab
    public Transform respawnPoint;       // assign a safe respawn point

    private GameObject currentPlayer;

    void Start()
    {
        RespawnPlayer();
    }

    public void PlayerDied()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.2f); // small delay to avoid immediate spike collision
        currentPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
    }

    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }
}
