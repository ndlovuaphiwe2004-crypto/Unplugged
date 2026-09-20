using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NewspaperIntro : MonoBehaviour
{
    public Image backgroundImage;       // Newspaper background
    public TMP_Text headlineText;       // HeadlineText object
    public TMP_Text storyText;          // StoryText object
    [TextArea] public string fullStory; // Storyline text
    public float backgroundFadeDuration = 2f;
    public float headlineDelay = 0.5f;
    public float headlineFadeDuration = 1f;
    public float typeSpeed = 0.05f;     // Delay between letters
    public GameObject continuePrompt;   // Press C to Continue prompt

    void Start()
    {
        if (continuePrompt != null)
            continuePrompt.SetActive(false);

        // Force headline invisible at start
        Color hlColor = headlineText.color;
        hlColor.a = 0f;
        headlineText.color = hlColor;

        // Prepare story text invisible
        storyText.text = "";

        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Fade background in
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

        // Small delay before headline
        yield return new WaitForSeconds(headlineDelay);

        // Fade headline in
        Color hlColor = headlineText.color;
        hlColor.a = 0f;
        headlineText.color = hlColor;

        elapsed = 0f;
        while (elapsed < headlineFadeDuration)
        {
            elapsed += Time.deltaTime;
            hlColor.a = Mathf.Clamp01(elapsed / headlineFadeDuration);
            headlineText.color = hlColor;
            yield return null;
        }

        // Typewriter effect for story
        storyText.text = "";
        foreach (char c in fullStory)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Show blinking prompt
        if (continuePrompt != null)
            continuePrompt.SetActive(true);
    }
}
