using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject quitConfirmPanel;

    [Header("Main Menu Root")]
    public CanvasGroup mainMenuGroup; // assign your main menu buttons group here

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 5");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuGroup.gameObject.SetActive(false); // hide all main menu buttons
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuGroup.gameObject.SetActive(true); // restore buttons
    }

    public void QuitGame()
    {
        quitConfirmPanel.SetActive(true);
        mainMenuGroup.gameObject.SetActive(false); // hide buttons
    }

    public void CancelQuit()
    {
        quitConfirmPanel.SetActive(false);
        mainMenuGroup.gameObject.SetActive(true); // restore buttons
    }
    public void MuteSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Mute();
    }


}
