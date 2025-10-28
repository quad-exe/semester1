using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour
{
    private ExitManager exitManager;

    public GameObject circlePlayer;
    public GameObject squarePlayer;



    public Collider2D circleExit;
    public Collider2D squareExit;

    public bool circleInside = false;
    public bool squareInside = false;

    public string nextSceneName;

    private void Start()
    {

    }



    public void CheckBothPlayers()
    {

        if (circleInside == true && squareInside == true)
        {
            Debug.Log("JUBII");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
