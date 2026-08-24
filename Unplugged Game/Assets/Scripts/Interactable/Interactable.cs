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
                ViewInfo();
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
        Debug.Log("Picked up: " + gameObject.name);
        playerInteraction.PickUpObject(gameObject);

        if (showInfoOnPickup && infoPanel != null)
        {
            StartCoroutine(ShowInfoAfterDelay());
        }
    }

    IEnumerator ShowInfoAfterDelay()
    {
        yield return new WaitForSeconds(infoDelay);

        infoPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (infoText != null)
        {
            infoText.text = instructionMessage;
        }

        Debug.Log("Info panel shown for: " + gameObject.name + " with message: " + instructionMessage);
    }

    void ViewInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (infoText != null)
            {
                infoText.text = instructionMessage;
            }
        }
    }

    void Open()
    {
        Debug.Log("Opened: " + gameObject.name);
    }

    void ShowPanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (infoText != null)
            {
                infoText.text = instructionMessage;
            }
        }
    }
}
