using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public TMP_Text promptText;
    public float blinkSpeed = 1f;

    private bool isVisible = true;
    private float timer = 0f;

    void Awake()
    {
        // Automatically grab the TMP_Text component if you forgot to assign it in the inspector
        if (promptText == null)
        {
            promptText = GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        // Use unscaledDeltaTime so the text keeps blinking even when the game is paused (Time.timeScale = 0)
        timer += Time.unscaledDeltaTime;

        if (timer >= blinkSpeed)
        {
            isVisible = !isVisible;
            if (promptText != null)
            {
                promptText.enabled = isVisible;
            }
            timer = 0f;
        }
    }
}