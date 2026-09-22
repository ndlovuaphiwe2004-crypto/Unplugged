using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene("MainMenu");
        }
    }
}
