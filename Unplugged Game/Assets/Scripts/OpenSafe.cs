using UnityEngine;
using TMPro;

public class OpenSafe : MonoBehaviour
{
    private Animator animator;
    private bool IsAtSafe = false;

    [Header("UI & Panels")]
    [SerializeField] private TextMeshProUGUI codeText;
    public GameObject codePanel;
    public GameObject gameOverPanel;

    [Header("Prompt Settings")]
    public TextMeshProUGUI interactionText;
    public string promptMessage = "Press E to open";

    private string code = "";
    public string correctCode = "1234";

    void Start()
    {
        animator = GetComponent<Animator>();
        if (codePanel != null) codePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (interactionText != null) interactionText.gameObject.SetActive(false);
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
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtSafe = true;

            // Tell PlayerInteraction to step back and let OpenSafe control the text
            PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.isNearSafe = true;
            }

            if (interactionText != null && (codePanel == null || !codePanel.activeSelf))
            {
                interactionText.text = promptMessage;
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtSafe = false;

            // Give control back to PlayerInteraction
            PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.isNearSafe = false;
            }

            if (codePanel != null)
            {
                codePanel.SetActive(false);
            }

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }

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

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (code.Length >= correctCode.Length)
        {
            code = "";
        }
    }

    public void ClearCode()
    {
        code = "";
    }
}