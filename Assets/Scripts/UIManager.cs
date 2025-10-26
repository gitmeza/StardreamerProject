using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    
    [Header("Main Menu")]
    public Button startButton;
    public Button quitButton;
    
    [Header("Game UI")]
    public Text scoreText;
    public Text livesText;
    public Text waveText;
    
    [Header("Pause Menu")]
    public Button resumeButton;
    public Button mainMenuButton;
    
    [Header("Game Over")]
    public Text finalScoreText;
    public Button restartButton;
    public Button mainMenuFromGameOverButton;
    
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
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ShowMainMenu);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (mainMenuFromGameOverButton != null)
            mainMenuFromGameOverButton.onClick.AddListener(ShowMainMenu);
    }
    
    void Update()
    {
        // Handle pause input
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        
        // Update game UI
        if (gamePanel != null && gamePanel.activeInHierarchy)
        {
            UpdateGameUI();
        }
    }
    
    void UpdateGameUI()
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
        SetActivePanel(mainMenuPanel);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void StartGame()
    {
        SetActivePanel(gamePanel);
        Time.timeScale = 1f;
        isPaused = false;
        
        // Start the game
        if (gameManager != null)
        {
            // Game manager will handle game start
        }
    }
    
    public void PauseGame()
    {
        SetActivePanel(pausePanel);
        Time.timeScale = 0f;
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        SetActivePanel(gamePanel);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public void ShowGameOver(int finalScore)
    {
        SetActivePanel(gameOverPanel);
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }
    }
    
    void SetActivePanel(GameObject activePanel)
    {
        // Deactivate all panels
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (gamePanel != null)
            gamePanel.SetActive(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        // Activate the specified panel
        if (activePanel != null)
            activePanel.SetActive(true);
    }
}
