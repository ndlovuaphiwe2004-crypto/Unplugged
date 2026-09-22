using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text storyText;         
    [TextArea] public string fullStory; 
    public float typeSpeed = 0.05f;

    public GameObject continuePrompt;   

    void Start()
    {
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

        if (continuePrompt != null)
            continuePrompt.SetActive(true);
    }
}
