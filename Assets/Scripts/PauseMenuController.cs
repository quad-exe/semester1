using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseMenuCanvas;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }
    }
    
    public void OnPause()
    {
        PauseMenuCanvas.SetActive(true);
    }


    public void OnContinueClick()
    {
        Debug.Log("Continue clicked");
        PauseMenuCanvas.SetActive(false);
    }

    public void OnMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("(Main menu) Stampe");
    }
}
