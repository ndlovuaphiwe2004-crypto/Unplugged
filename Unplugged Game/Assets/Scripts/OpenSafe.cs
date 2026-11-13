using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    [Header("Victory Effects")]
    [SerializeField] private ParticleSystem confettiParticleSystem;

    [Header("Feedback Settings")]
    [SerializeField] private Image panelBackgroundImage; // Drag your codePanel's Image component here
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color errorColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private string code = "";
    public string correctCode = "1234";

    void Start()
    {
        animator = GetComponent<Animator>();
        if (codePanel != null) codePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (interactionText != null) interactionText.gameObject.SetActive(false);

        if (confettiParticleSystem != null)
        {
            confettiParticleSystem.Stop();
        }

        // Auto-grab Image component from codePanel if not assigned
        if (panelBackgroundImage == null && codePanel != null)
        {
            panelBackgroundImage = codePanel.GetComponent<Image>();
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

            if (confettiParticleSystem != null)
            {
                confettiParticleSystem.Stop();
                confettiParticleSystem.Clear();
                confettiParticleSystem.Play();
            }

            GameClearButton gameClear = FindAnyObjectByType<GameClearButton>();

            if (gameClear != null)
            {
                gameClear.ShowClearedPanel();
            }
        }
        else if (code.Length >= correctCode.Length)
        {
            // Trigger red flash and clear wrong code
            StartCoroutine(FlashRedAndReset());
        }
    }

    private IEnumerator FlashRedAndReset()
    {
        if (panelBackgroundImage != null)
        {
            panelBackgroundImage.color = errorColor;
        }

        yield return new WaitForSeconds(flashDuration);

        if (panelBackgroundImage != null)
        {
            panelBackgroundImage.color = normalColor;
        }

        code = "";
    }

    public void ClearCode()
    {
        code = "";
    }
}