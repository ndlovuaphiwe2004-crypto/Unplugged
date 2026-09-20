using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterFade : MonoBehaviour
{
    public TMP_Text storyText;          // Assign your StoryText
    [TextArea] public string fullStory; // Paste storyline here
    public float typeSpeed = 0.05f;     // Delay between letters
    public float fadeDuration = 0.1f;   // Fade time per letter

    void Start()
    {
        storyText.text = fullStory;
        storyText.maxVisibleCharacters = 0;
        StartCoroutine(TypeStory());
    }

    IEnumerator TypeStory()
    {
        int totalChars = fullStory.Length;
        for (int i = 0; i < totalChars; i++)
        {
            storyText.maxVisibleCharacters = i + 1;

            // Optional: fade effect per character
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                storyText.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }

            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
