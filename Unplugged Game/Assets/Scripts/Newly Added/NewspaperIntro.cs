using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NewspaperIntro : MonoBehaviour
{
    public Image backgroundImage;
    public TMP_Text storyText;
    [TextArea] public string fullStory;
    public float backgroundFadeDuration = 2f;
    public float typeSpeed = 0.05f;
    public GameObject continuePrompt;

    void Start()
    {
        if (continuePrompt != null)
            continuePrompt.SetActive(false);

        storyText.text = "";

        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        Color bgColor = backgroundImage.color;
        bgColor.a = 0f;
        backgroundImage.color = bgColor;

        float elapsed = 0f;
        while (elapsed < backgroundFadeDuration)
        {
            elapsed += Time.deltaTime;
            bgColor.a = Mathf.Clamp01(elapsed / backgroundFadeDuration);
            backgroundImage.color = bgColor;
            yield return null;
        }

        storyText.text = "";
        foreach (char c in fullStory)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        if (continuePrompt != null)
            continuePrompt.SetActive(true);
    }
}
