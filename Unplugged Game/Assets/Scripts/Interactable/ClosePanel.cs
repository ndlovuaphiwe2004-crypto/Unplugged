using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject panelToClose;
    private Interactable currentInteractable;

    void Update()
    {
        if (panelToClose.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            Close();
        }
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    public void Close()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (currentInteractable != null && currentInteractable.riddleIndex >= 0)
            {
                RiddleManager.Instance.RiddleSolved();
                Debug.Log("Solved riddle " + currentInteractable.riddleIndex +
                          ". Next index unlocked: " + (currentInteractable.riddleIndex + 1));
            }
        }
    }
}
