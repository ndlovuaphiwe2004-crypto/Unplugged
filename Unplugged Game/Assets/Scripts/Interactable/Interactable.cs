using UnityEngine;
using System.Collections;
using TMPro;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactionMessage = "Press F to interact";

    public enum InteractionType
    {
        PickUp,
        ViewInfo,
        Open,
        ShowPanel
    }

    public InteractionType interactionType;

    [Header("Optional")]
    public GameObject infoPanel;
    public TMP_Text infoText;
    public bool showInfoOnPickup = false;
    public float infoDelay = 2f;
    public string instructionMessage;

    public void Interact(PlayerInteraction playerInteraction)
    {
        switch (interactionType)
        {
            case InteractionType.PickUp:
                PickUp(playerInteraction);
                break;
            case InteractionType.ViewInfo:
                ViewInfo(playerInteraction);
                break;
            case InteractionType.Open:
                Open();
                break;
            case InteractionType.ShowPanel:
                ShowPanel();
                break;
        }
    }

    public int riddleIndex = -1;

    void PickUp(PlayerInteraction playerInteraction)
    {
        playerInteraction.PickUpObject(gameObject);

        if (riddleIndex >= 0 && RiddleManager.Instance.CanShowRiddle(riddleIndex))
        {
            if (showInfoOnPickup && infoPanel != null)
            {
                StartCoroutine(ShowInfoAfterDelay(playerInteraction));
            }
        }
    }

    public void CloseInfoPanel(PlayerInteraction playerInteraction)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (riddleIndex >= 0)
            {
                RiddleManager.Instance.RiddleSolved();
            }

            var movement = playerInteraction.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = true;
        }
    }


    IEnumerator ShowInfoAfterDelay(PlayerInteraction playerInteraction)
    {
        yield return new WaitForSeconds(infoDelay);
        ShowInfoPanel(playerInteraction);
    }

    void ViewInfo(PlayerInteraction playerInteraction)
    {
        ShowInfoPanel(playerInteraction);
    }

    void ShowInfoPanel(PlayerInteraction playerInteraction)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (infoText != null)
            {
                infoText.text = instructionMessage;
            }

            ClosePanel closePanel = infoPanel.GetComponent<ClosePanel>();
            if (closePanel != null)
            {
                closePanel.SetCurrentInteractable(this);
            }

            var movement = playerInteraction.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = false;
        }
    }

    void Open()
    {
        Debug.Log("Opened: " + gameObject.name);
    }

    void ShowPanel()
    {
        GameClearButton clearButton = GetComponent<GameClearButton>();
        if (clearButton != null)
        {
            clearButton.ShowClearedPanel();
        }
    }
}
