using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene reload

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseScreen; // Assign your PauseScreen GameObject in inspector

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseScreen != null)
            pauseScreen.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Resume button functionality
    public void ResumeGame()
    {
        isPaused = false;
        if (pauseScreen != null)
            pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    // Restart button functionality
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Make sure timeScale is normal
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}

