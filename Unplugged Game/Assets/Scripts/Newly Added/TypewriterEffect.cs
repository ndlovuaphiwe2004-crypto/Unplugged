using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text storyText;          // Assign your StoryText
    [TextArea] public string fullStory; // Paste storyline here
    public float typeSpeed = 0.05f;

    public GameObject continuePrompt;   // Assign your ContinuePrompt object

    void Start()
    {
        // Hide the prompt at the start
        if (continuePrompt != null)
            continuePrompt.SetActive(false);

        StartCoroutine(TypeStory());
    }

    IEnumerator TypeStory()
    {
        storyText.text = "";
        foreach (char c in fullStory)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Show the prompt once typing is finished
        if (continuePrompt != null)
            continuePrompt.SetActive(true);
    }
}
