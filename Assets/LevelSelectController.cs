using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    public void onTutorialClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
    
    public void OnLvl1Click()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnLvl2Click()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void OnLvl3Click()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void OnLvl4Click()
    {
        SceneManager.LoadScene("Level 4");
    }

    public void OnLvl5Click()
    {
        SceneManager.LoadScene("Level 5");
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("(Main menu) Stampe");
    }
}