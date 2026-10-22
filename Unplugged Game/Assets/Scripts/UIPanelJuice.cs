using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanelJuice : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Coroutine currentAnimation;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        // Play pop-up animation every time the panel opens
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateOpen());
    }

    IEnumerator AnimateOpen()
    {
        float duration = 0.25f;
        float elapsed = 0f;

        // Start small and invisible
        transform.localScale = Vector3.one * 0.8f;
        canvasGroup.alpha = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Uses unscaled time so it works even if game is paused
            float t = elapsed / duration;

            // Smooth ease-out curve with a slight overshoot bounce
            float scale = Mathf.Lerp(0.8f, 1f, Mathf.Sin(t * Mathf.PI * 0.5f));
            if (t > 0.8f) scale = Mathf.Lerp(scale, 1f, (t - 0.8f) / 0.2f); // subtle pop settle

            transform.localScale = Vector3.one * scale;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
    }
}