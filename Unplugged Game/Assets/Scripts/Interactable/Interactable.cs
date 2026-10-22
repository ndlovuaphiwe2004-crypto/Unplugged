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
    public float infoDelay = 2f;
    public string instructionMessage;

    public int riddleIndex = -1;

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

    void PickUp(PlayerInteraction playerInteraction)
    {
        // 1. Always pick up the object first
        playerInteraction.PickUpObject(gameObject);

        // 2. ONLY set as a clue if it has a valid riddle AND an info panel assigned
        if (HasClue() && RiddleManager.Instance.CanShowRiddle(riddleIndex))
        {
            playerInteraction.SetClueInteractable(this);
        }
        else
        {
            // If it's a normal pickup with no clue, clear any clue reference
            playerInteraction.SetClueInteractable(null);
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

            // UNFREEZE THE GAME
            Time.timeScale = 1f;
        }
    }

    IEnumerator ShowInfoAfterDelay(PlayerInteraction playerInteraction)
    {
        yield return new WaitForSeconds(infoDelay);
        OpenInfoPanel(playerInteraction);
    }

    void ViewInfo(PlayerInteraction playerInteraction)
    {
        // For static items (notes/signs on walls) that open directly on F
        OpenInfoPanel(playerInteraction);
    }

    public void OpenInfoPanel(PlayerInteraction playerInteraction)
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

            // FREEZE THE GAME
            Time.timeScale = 0f;
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

    // Returns true only if it's meant to be a collectible clue with a riddle and panel
    public bool HasClue()
    {
        return riddleIndex >= 0 && infoPanel != null;
    }
}