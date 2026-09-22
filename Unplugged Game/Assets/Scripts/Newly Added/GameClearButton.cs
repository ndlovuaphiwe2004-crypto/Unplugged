using UnityEngine;
using System.Collections;

public class GameClearButton : MonoBehaviour
{
    public GameObject clearedPanel;
    public float fadeDuration = 1f;

    void Start()
    {
        if (clearedPanel != null)
        {
            clearedPanel.SetActive(false);
        }
    }

    public void ShowClearedPanel()
    {
        if (clearedPanel != null)
        {
            clearedPanel.SetActive(true);
            StartCoroutine(FadeInPanel(clearedPanel, fadeDuration));

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

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
            elapsed += Time.unscaledDeltaTime; // unaffected by pause
            cg.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        cg.alpha = 1f;
    }
}
