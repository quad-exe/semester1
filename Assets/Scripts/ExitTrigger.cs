using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [SerializeField] private ExitManager exitManager;

    public int playerNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exitManager = FindAnyObjectByType<ExitManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Circle") && playerNumber == 1)
        {
            exitManager.circleInside = true;
            Debug.Log("CIRCLE");
        }

        if (collision.CompareTag("Square") && playerNumber == 2)
        {
            exitManager.squareInside = true;
            Debug.Log("SQUARE");
        }


        exitManager.CheckBothPlayers();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Circle") && playerNumber == 1)
        {
            exitManager.circleInside = false;
            Debug.Log("Farvel circle");
        }

        if (collision.CompareTag("Square") && playerNumber == 2)
        {
            exitManager.squareInside = false;
            Debug.Log("Farvel square");
        }
    }
}
