using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    public void OnLvl1Click()
    {
        SceneManager.LoadScene("");
    }

    public void OnLvl2Click()
    {
        SceneManager.LoadScene("");
    }

    public void OnLvl3Click()
    {
        SceneManager.LoadScene("");
    }

    public void OnLvl4Click()
    {
        SceneManager.LoadScene("");
    }

    public void OnLvl5Click()
    {
        SceneManager.LoadScene("");
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("(Main menu) Stampe");
    }
}