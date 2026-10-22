using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject quitConfirmPanel;

    [Header("Main Menu Root")]
    public CanvasGroup mainMenuGroup; 

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 5");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuGroup.gameObject.SetActive(false); 
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuGroup.gameObject.SetActive(true); 
    }

    
    public void QuitGame()
    {
        quitConfirmPanel.SetActive(true);
        mainMenuGroup.gameObject.SetActive(false); 
    }

    public void CancelQuit()
    {
        quitConfirmPanel.SetActive(false);
        mainMenuGroup.gameObject.SetActive(true); 
    }

    public void ConfirmQuit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void MuteSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Mute();
    }
}