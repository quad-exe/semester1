using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform targetPosition;   // sideways destination
    public float downDistance = 3f;    // how far down it moves
    public float moveSpeed = 2f;
    public float waitTime = 1f;        // pause at bottom

    private Vector3 startPosition;
    private Vector3 downPosition;
    private int step = 0;              // current step in path
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
                MoveTowards(startPosition, 5);
                break;

            case 5: // finished
                activated = false;
                step = 0;
                downPosition = Vector3.zero;
                break;
        }
    }

    private void MoveTowards(Vector3 destination, int nextStep)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            step = nextStep;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player2"))
        {
            // Parent player to platform
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player2"))
        {
            // Remove parent when leaving
            collision.collider.transform.SetParent(null);
        }
    }
}
