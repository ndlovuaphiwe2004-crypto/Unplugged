using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public TMP_Text promptText;  
    public float blinkSpeed = 1f; 

    private bool isVisible = true;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= blinkSpeed)
        {
            isVisible = !isVisible;
            promptText.enabled = isVisible;
            timer = 0f;
        }
    }
}
