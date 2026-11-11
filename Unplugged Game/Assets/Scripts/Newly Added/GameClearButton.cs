using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameClearButton : MonoBehaviour
{
    [Header("UI")]
    public GameObject clearedPanel;

    [Header("Animation")]
    public float fadeDuration = 1f;

    [Header("Delay Before Showing Panel")]
    public float panelDelay = 3f;

    private bool panelShown = false;

    void Start()
    {
        panelShown = false;

        if (clearedPanel != null)
            clearedPanel.SetActive(false);
    }

    void Update()
    {
        if (panelShown && Input.GetKeyDown(KeyCode.H))
        {
            ReturnToMenu();
        }
    }

    public void ShowClearedPanel()
    {
        if (!panelShown)
            StartCoroutine(ShowClearedPanelRoutine());
    }

    void OnEnable()
    {
        panelShown = false;
    }

    IEnumerator ShowClearedPanelRoutine()
    {
        panelShown = true;

        // Wait before showing panel
        yield return new WaitForSecondsRealtime(panelDelay);

        clearedPanel.SetActive(true);

        StartCoroutine(FadeInPanel(clearedPanel, fadeDuration));

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement movement = FindObjectOfType<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        PlayerInteraction interaction = FindObjectOfType<PlayerInteraction>();
        if (interaction != null)
            interaction.enabled = false;

        Debug.Log("Game Cleared panel shown!");
    }

    IEnumerator FadeInPanel(GameObject panel, float duration)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>();

        cg.alpha = 0f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        cg.alpha = 1f;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}