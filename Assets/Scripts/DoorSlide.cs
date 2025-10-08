using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CooperativeDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorPanel;
    public Vector3 openOffset;
    public float openSpeed = 2f;

    [Header("Switch Settings")]
    public SwitchActivation switchTrigger;

    [Header("Exit Trigger")]
    public Collider2D exitTrigger;

    [Header("Win Settings")]
    public GameObject winScreen;
    public float walkSpeed = 2f;
    public Transform exitPoint;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool winSequenceStarted = false;

    private HashSet<string> playersAtExit = new HashSet<string>();

    void Start()
    {
        closedPosition = doorPanel.position;
        openPosition = closedPosition + openOffset;

        if (winScreen != null)
            winScreen.SetActive(false);
    }

    void Update()
    {
        // Open the door when the switch is activated
        if (switchTrigger != null && switchTrigger.isActivated)
        {
            doorPanel.position = Vector3.Lerp(doorPanel.position, openPosition, Time.deltaTime * openSpeed);

            // Only start win sequence if door is open AND both players are at the exit
            if (!winSequenceStarted &&
                Vector3.Distance(doorPanel.position, openPosition) < 0.05f &&
                playersAtExit.Contains("Player1") &&
                playersAtExit.Contains("Player2"))
            {
                winSequenceStarted = true;
                StartCoroutine(WinSequence());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playersAtExit.Add(other.tag);
            Debug.Log(other.name + " entered exit trigger.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playersAtExit.Remove(other.tag);
            Debug.Log(other.name + " left exit trigger.");
        }
    }

    private IEnumerator WinSequence()
    {
        Debug.Log("Win sequence started!");
        if (winScreen != null)
            winScreen.SetActive(true);

        GameObject player1 = GameObject.FindWithTag("Player1");
        GameObject player2 = GameObject.FindWithTag("Player2");

        List<GameObject> players = new List<GameObject>();
        if (player1 != null) players.Add(player1);
        if (player2 != null) players.Add(player2);

        bool allPlayersAtExitPoint = false;
        while (!allPlayersAtExitPoint)
        {
            allPlayersAtExitPoint = true;
            foreach (var player in players)
            {
                if (player == null) continue;

                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    exitPoint.position,
                    walkSpeed * Time.deltaTime
                );

                if (Vector3.Distance(player.transform.position, exitPoint.position) > 0.05f)
                    allPlayersAtExitPoint = false;
            }

            yield return null;
        }

        Debug.Log("All players have exited! Game won.");
        // Optional: load next level or trigger victory event
    }
}
