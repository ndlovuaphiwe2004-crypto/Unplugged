using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueScript : MonoBehaviour
{
    public GameObject continuePrompt;
    public TerminalIntro terminalIntro; // Drag your TerminalIntro script reference here in the Inspector

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
                // 1. Stop all typewriter audio immediately if assigned
                if (terminalIntro != null && terminalIntro.typewriterAudio != null)
                {
                    terminalIntro.typewriterAudio.Stop();
                    terminalIntro.StopAllCoroutines();
                }

                // 2. Fallback: Stop any active audio sources globally in the scene
                AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
                foreach (AudioSource audio in allAudioSources)
                {
                    audio.Stop();
                }

                // 3. Load the Main Menu scene
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