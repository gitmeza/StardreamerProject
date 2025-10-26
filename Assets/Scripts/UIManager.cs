using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject gameInfo;

    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI finalScoreText;

    [Header("Buttons")]
    public Button startButton;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    private GameManager gameManager;
    private bool isPaused = false;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        SetupButtons();
        ShowMainMenu();
    }

    void SetupButtons()
    {
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (gameManager != null)
        {
            if (scoreText != null)
                scoreText.text = "Score: " + gameManager.score;

            if (livesText != null)
                livesText.text = "Lives: " + gameManager.playerLives;

            if (waveText != null)
                waveText.text = "Wave: " + gameManager.currentWave;
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (pausePanel != null)
            pausePanel.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
            gameInfo.SetActive(true);
        if (gameManager != null)
        {
            gameManager.RespawnPlayer();
        }
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + finalScore;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
