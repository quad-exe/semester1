using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpawningScript : MonoBehaviour
{
    public GameObject circlePlayer;
    public GameObject squarePlayer;

    public Transform circleSpawnPosition;
    public Transform squareSpawnPosition;

    private GameObject circleInstance;
    private GameObject squareInstance;


    void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        if (circleInstance != null)
        {
            Destroy(circleInstance);
        }
        if (squareInstance != null)
        {
            Destroy(squareInstance);
        }

        circleInstance = Instantiate(circlePlayer, circleSpawnPosition.position, circleSpawnPosition.rotation);
        squareInstance = Instantiate(squarePlayer, squareSpawnPosition.position, squareSpawnPosition.rotation);
    }
}
