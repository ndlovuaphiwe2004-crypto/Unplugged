using UnityEngine;
using UnityEngine.UI;

public class IntroPanelController : MonoBehaviour
{
    public GameObject introPanel; 
    public Button continueButton;

    void Start()
    {
        introPanel.SetActive(true); 
        Time.timeScale = 0f;    

        if (continueButton != null)
            continueButton.onClick.AddListener(ClosePanel);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (introPanel.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        introPanel.SetActive(false);
        Time.timeScale = 1f;       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
