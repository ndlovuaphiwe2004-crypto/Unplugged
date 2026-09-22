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

    public CanvasGroup[] buttonGroups;
    public float buttonFadeDuration = 0.5f;
    public float buttonStaggerDelay = 0.3f;
    public float dropDistance = 200f;

    public AnimationCurve dropCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float cursorRevealDelay = 0.5f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        foreach (CanvasGroup group in buttonGroups)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;

            RectTransform rt = group.GetComponent<RectTransform>();
            rt.anchoredPosition += new Vector2(0, dropDistance);
        }

        titleText.text = "";
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        foreach (char c in gameTitle)
        {
            titleText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        Vector3 startPos = titleText.rectTransform.anchoredPosition;
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            titleText.rectTransform.anchoredPosition =
                Vector3.Lerp(startPos, targetPosition, Mathf.SmoothStep(0f, 1f, elapsed / moveDuration));
            yield return null;
        }

        for (int i = buttonGroups.Length - 1; i >= 0; i--)
        {
            yield return StartCoroutine(DropInButton(buttonGroups[i]));
            yield return new WaitForSeconds(buttonStaggerDelay);
        }

        yield return new WaitForSeconds(cursorRevealDelay);

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

            group.alpha = t;

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
