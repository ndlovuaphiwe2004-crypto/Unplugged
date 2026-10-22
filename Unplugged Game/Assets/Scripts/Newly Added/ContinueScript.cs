using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueScript : MonoBehaviour
{
    public GameObject continuePrompt;

    void Start()
    {
        if (continuePrompt != null)
            continuePrompt.SetActive(false);

        // Show the prompt after 25 seconds
        Invoke("ShowPrompt", 25f);
    }

    void Update()
    {
        if (continuePrompt != null && continuePrompt.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void ShowPrompt()
    {
        if (continuePrompt != null)
            continuePrompt.SetActive(true);
    }
}
