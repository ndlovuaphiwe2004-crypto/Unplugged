using UnityEngine;
using UnityEngine.UI;

public class IntroPanelController : MonoBehaviour
{
    public GameObject introPanel;   // assign your UI panel in Inspector
    public Button continueButton;   // assign the button from the panel

    void Start()
    {
        introPanel.SetActive(true); // show panel at start
        Time.timeScale = 0f;        // pause gameplay

        // Hook up button click
        if (continueButton != null)
            continueButton.onClick.AddListener(ClosePanel);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Allow pressing C as alternative
        if (introPanel.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        introPanel.SetActive(false);
        Time.timeScale = 1f;        // resume gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
