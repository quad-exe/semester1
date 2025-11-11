using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform targetPosition;    // Horizontal destination
    public float downDistance = 3f;     // How far down it drops
    public float moveSpeed = 2f;        // Movement speed
    public float waitTime = 1f;         // Pause at bottom

    [Header("Platform Tags")]
    public string[] rideableTags = { "PlatformA", "PlatformB" }; // Platforms Player2 can ride

    private Vector3 startPosition;
    private Vector3 downPosition;
    private int step = 0;               // Step in movement sequence
    private bool activated = false;
    private float waitTimer = 0f;

    private Vector3 lastPosition;

    void Start()
    {
        startPosition = transform.position;

        if (targetPosition == null)
            Debug.LogError("No target position assigned!");

        downPosition = targetPosition.position + Vector3.down * downDistance;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!activated || targetPosition == null) return;

        Vector3 oldPos = transform.position;

        switch (step)
        {
            case 0: // Move start → target
                MoveTowards(targetPosition.position, 1);
                break;

            case 1: // Move target → down
                MoveTowards(downPosition, 2);
                break;

            case 2: // Wait at bottom
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    waitTimer = 0f;
                    step = 3;
                }
                break;

            case 3: // Move down → target (back up)
                MoveTowards(targetPosition.position, 4);
                break;

            case 4: // Move target → start
                MoveTowards(startPosition, -1);
                break;

            case -1: // Finished
                activated = false;
                step = 0;
                break;
        }

        // Apply platform movement delta to any players on top
        Vector3 delta = transform.position - lastPosition;
        MoveRidingPlayers(delta);

        lastPosition = transform.position;
    }

    private void MoveTowards(Vector3 destination, int nextStep)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.01f)
            step = nextStep;
    }

    private void MoveRidingPlayers(Vector3 delta)
    {
        foreach (string playerTag in new string[] { "Circle", "Square" })
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
            foreach (var p in players)
            {
                // Check if player is standing on the platform
                Collider2D playerCol = p.GetComponent<Collider2D>();
                Collider2D platformCol = GetComponent<Collider2D>();
                if (playerCol != null && platformCol != null)
                {
                    if (playerCol.IsTouching(platformCol))
                    {
                        p.transform.position += delta;
                    }
                }
            }
        }
    }

    public void Activate()
    {
        if (!activated)
        {
            activated = true;
            step = 0;
        }
    }
}

