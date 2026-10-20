using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterFade : MonoBehaviour
{
    public TMP_Text storyText;          
    [TextArea] public string fullStory; 
    public float typeSpeed = 0.05f;     
    public float fadeDuration = 0.1f; 

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
