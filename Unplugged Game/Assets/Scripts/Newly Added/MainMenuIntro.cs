using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MainMenuIntro : MonoBehaviour
{
    public TMP_Text titleText;
    public string gameTitle = "UNPLUGGED";
    public float typeSpeed = 0.1f;
    public float moveDuration = 1f;
    public Vector3 targetPosition;

    public CanvasGroup[] buttonGroups;   // Assign buttons top-to-bottom in Inspector
    public float buttonFadeDuration = 0.5f;
    public float buttonStaggerDelay = 0.3f;
    public float dropDistance = 200f;    // How far above they start

    public AnimationCurve dropCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float cursorRevealDelay = 0.5f; // Delay before cursor reappears

    void Start()
    {
        // Hide and lock cursor immediately
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Hide buttons at start
        foreach (CanvasGroup group in buttonGroups)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;

            RectTransform rt = group.GetComponent<RectTransform>();
            rt.anchoredPosition += new Vector2(0, dropDistance); // start above
        }

        titleText.text = "";
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Typewriter effect for title
        foreach (char c in gameTitle)
        {
            titleText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Move title upward
        Vector3 startPos = titleText.rectTransform.anchoredPosition;
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            titleText.rectTransform.anchoredPosition =
                Vector3.Lerp(startPos, targetPosition, Mathf.SmoothStep(0f, 1f, elapsed / moveDuration));
            yield return null;
        }

        // Drop buttons in reverse order (bottom first)
        for (int i = buttonGroups.Length - 1; i >= 0; i--)
        {
            yield return StartCoroutine(DropInButton(buttonGroups[i]));
            yield return new WaitForSeconds(buttonStaggerDelay);
        }

        // Small delay before cursor reappears
        yield return new WaitForSeconds(cursorRevealDelay);

        // Reveal and unlock cursor once all buttons are ready
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator DropInButton(CanvasGroup group)
    {
        RectTransform rt = group.GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, startPos.y - dropDistance);

        float elapsed = 0f;
        while (elapsed < buttonFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / buttonFadeDuration);

            // Fade in
            group.alpha = t;

            // Drop motion using curve
            float curveValue = dropCurve.Evaluate(t);
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);

            yield return null;
        }

        rt.anchoredPosition = endPos;
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }
}
