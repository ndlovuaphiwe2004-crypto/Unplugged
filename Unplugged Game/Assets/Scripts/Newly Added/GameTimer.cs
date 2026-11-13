using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 420f;
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;

    private float remainingTime;
    private bool timerRunning = false;
    private bool gameOverActive = false;

    void Start()
    {
        remainingTime = totalTime;
        StartTimer();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (timerText == null)
        {
            Debug.LogError("GAME TIMER ERROR: timerText slot is empty in the Inspector!");
        }
        else
        {
            Debug.Log("GAME TIMER SUCCESS: timerText is linked to " + timerText.gameObject.name);
        }
    }

    void Update()
    {
        if (!gameObject.scene.isLoaded) return;

        if (timerRunning)
        {
            remainingTime -= Time.unscaledDeltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                timerRunning = false;
                ShowGameOver();
            }

            UpdateTimerUI();
        }

        if (gameOverActive)
        {
            Time.timeScale = 0f;
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
        remainingTime = totalTime;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            string timeString = $"{minutes:00}:{seconds:00}";

            timerText.text = timeString;

            // Uncomment this line temporarily if you want to spam the console and see if it's updating:
            // Debug.Log("Updating timer text to: " + timeString);
        }
        else
        {
            Debug.LogWarning("UpdateTimerUI skipped because timerText is null!");
        }
    }

    void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverActive = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}