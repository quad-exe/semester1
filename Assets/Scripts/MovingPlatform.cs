using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform targetPosition;   // sideways destination
    public float downDistance = 3f;    // how far down it moves
    public float moveSpeed = 2f;
    public float waitTime = 1f;        // pause time at bottom

    private Vector3 startPosition;
    private Vector3 downPosition;
    private int step = 0; // which step of the path we're on
    private bool activated = false;
    private float waitTimer = 0f;

    void Start()
    {
        startPosition = transform.position;
        if (targetPosition == null)
        {
            Debug.LogError("No target position assigned to platform!");
        }
    }

    void Update()
    {
        if (!activated || targetPosition == null) return;

        switch (step)
        {
            case 0: // move start → target
                MoveTowards(targetPosition.position, 1);
                break;

            case 1: // move target → down
                if (downPosition == Vector3.zero)
                    downPosition = targetPosition.position + Vector3.down * downDistance;

                MoveTowards(downPosition, 2);
                break;

            case 2: // wait at bottom
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    waitTimer = 0f;
                    step = 3;
                }
                break;

            case 3: // move down → target (back up)
                MoveTowards(targetPosition.position, 4);
                break;

            case 4: // move target → start
                MoveTowards(startPosition, -1); // -1 = stop after done
                break;
        }
    }

    private void MoveTowards(Vector3 destination, int nextStep)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            step = nextStep;
            if (step == -1) // finished route
            {
                activated = false;
                step = 0;
                downPosition = Vector3.zero; // reset
            }
        }
    }

    public void Activate()
    {
        if (!activated) // only trigger if not already running
        {
            activated = true;
            step = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player2"))
        {
            // Parent the player to the platform
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player2"))
        {
            // Remove parent when leaving the platform
            collision.collider.transform.SetParent(null);
        }
    }

}
