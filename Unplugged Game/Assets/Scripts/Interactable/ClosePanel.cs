using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject panelToClose;

    void Update()
    {
        // Allow player to press C to close
        if (panelToClose.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            Close();
        }
    }

    public void Close()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
            Time.timeScale = 1f; // resume game
            Cursor.lockState = CursorLockMode.Locked; // re-lock mouse
            Cursor.visible = false;                   // hide cursor
            Debug.Log("Panel closed: " + panelToClose.name);
        }
    }
}
