using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameClearButton : MonoBehaviour
{
    public GameObject clearedPanel;
    public float fadeDuration = 1f;
    private bool panelShown = false;

    void Start()
    {
        if (clearedPanel != null)
        {
            clearedPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (panelShown && clearedPanel.activeSelf && Input.GetKeyDown(KeyCode.H))
        {
            ReturnToMenu();
        }
    }

    public void ShowClearedPanel()
    {
        if (panelShown) return;
        panelShown = true;

        if (clearedPanel != null)
        {
            clearedPanel.SetActive(true);
            StartCoroutine(FadeInPanel(clearedPanel, fadeDuration));

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PlayerMovement movement = FindObjectOfType<PlayerMovement>();
            if (movement != null) movement.enabled = false;

            PlayerInteraction interaction = FindObjectOfType<PlayerInteraction>();
            if (interaction != null) interaction.enabled = false;

            Debug.Log("Game Cleared panel shown!");
        }
    }

    IEnumerator FadeInPanel(GameObject panel, float duration)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();

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
