using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject LoseMenu;
    [SerializeField] private GameObject WinMenu;

    void Start()
    {
        SetPaused(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPaused(!IsPaused);
        }
    }

    public void SetPaused(bool paused)
    {
        IsPaused = paused;

        // UI
        if (pauseMenu != null)
            pauseMenu.SetActive(paused);

        // Tempo di gioco
        Time.timeScale = paused ? 0f : 1f;

        // Mouse
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    // Comodo per bottoni UI
    public void ResumeButton() => SetPaused(false);

    // Se hai un bottone "Quit"
    public void QuitGame()
    {
        //Quit in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //function to menu from pause/win/lose screen
    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    //function to restart the game from pause/win/lose screen
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level");
    }

    public void lose(bool paused)
    {
        // UI
        if (LoseMenu != null)
            LoseMenu.SetActive(true);

        // Tempo di gioco
        Time.timeScale = paused ? 0f : 1f;

        // Mouse
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    public void win(bool paused)
    {
        // UI
        if (WinMenu != null)
            WinMenu.SetActive(true);

        // Tempo di gioco
        Time.timeScale = paused ? 0f : 1f;

        // Mouse
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }
}
