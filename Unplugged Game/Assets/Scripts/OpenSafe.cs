using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenSafe : MonoBehaviour
{
    private Animator animator;

    private bool IsAtSafe = false;

    [SerializeField] private TextMeshProUGUI codeText;
    private string code = "";
    public string correctCode = "1234";
    public GameObject codePanel;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (codePanel != null)
        {
            codePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (codeText != null)
        {
            codeText.text = code;
        }

        if (Input.GetKeyDown(KeyCode.E) && IsAtSafe)
        {
            if (codePanel != null)
            {
                codePanel.SetActive(true);
                // Unlock cursor so player can click keypad buttons
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtSafe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtSafe = false;
            if (codePanel != null)
            {
                codePanel.SetActive(false);
            }
            // Lock cursor back for first-person camera movement
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddDigit(string digit)
    {
        if (code.Length < correctCode.Length)
        {
            code += digit;
            CheckCode();
        }
    }

    private void CheckCode()
    {
        if (code == correctCode)
        {
            if (animator != null)
            {
                animator.SetTrigger("OpenSafe");
            }

            if (codePanel != null)
            {
                codePanel.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (code.Length >= correctCode.Length)
        {
            // Reset code after wrong attempt
            code = "";
        }
    }

    public void ClearCode()
    {
        code = "";
    }
}