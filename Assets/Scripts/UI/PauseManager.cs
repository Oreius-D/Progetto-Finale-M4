using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject LoseMenu;
    [SerializeField] private GameObject WinMenu;

    // Prevent ESC from toggling pause while Win/Lose screens are active.
    private bool isEndScreenActive = false;

    void Start()
    {
        // Start in gameplay mode (no menus, time running, cursor locked).
        ShowNone();
        ApplyPause(false);
    }

    void Update()
    {
        // If an end screen is active, ignore ESC.
        if (isEndScreenActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPaused(!IsPaused);
        }
    }

    // ===== Core helpers =====

    // Applies the actual pause effects (time scale + cursor) and updates IsPaused.
    private void ApplyPause(bool paused)
    {
        IsPaused = paused;

        Time.timeScale = paused ? 0f : 1f;

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    // Ensures only one menu is visible at a time.
    private void ShowOnly(GameObject menuToShow)
    {
        if (pauseMenu != null) pauseMenu.SetActive(menuToShow == pauseMenu);
        if (LoseMenu != null) LoseMenu.SetActive(menuToShow == LoseMenu);
        if (WinMenu != null) WinMenu.SetActive(menuToShow == WinMenu);
    }

    private void ShowNone()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (LoseMenu != null) LoseMenu.SetActive(false);
        if (WinMenu != null) WinMenu.SetActive(false);
    }

    // ===== Public API =====

    public void SetPaused(bool paused)
    {
        // Normal pause is not an end screen.
        isEndScreenActive = false;

        ShowOnly(paused ? pauseMenu : null);
        ApplyPause(paused);

        // If unpausing, hide everything.
        if (!paused) ShowNone();
    }

    public void ResumeButton() => SetPaused(false);

    // Call when player loses
    public void ShowLoseScreen()
    {
        isEndScreenActive = true;
        ShowOnly(LoseMenu);
        ApplyPause(true); // freeze game
    }

    // Call when player wins
    public void ShowWinScreen()
    {
        isEndScreenActive = true;
        ShowOnly(WinMenu);
        ApplyPause(true); // freeze game
    }

    public void MainMenu()
    {
        // Safe reset before loading scenes
        isEndScreenActive = false;
        ShowNone();
        ApplyPause(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        isEndScreenActive = false;
        ShowNone();
        ApplyPause(false);
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}