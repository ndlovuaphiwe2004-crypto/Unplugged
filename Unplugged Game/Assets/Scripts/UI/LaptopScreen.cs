using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaptopScreen : MonoBehaviour
{
    public Image screenOverlay;
    public GameObject terminalPanel;
    public TerminalIntro terminalIntro;
    public float fadeSpeed = 1f;

    void Start()
    {
        StartCoroutine(ScreenOn());
    }

    IEnumerator ScreenOn()
    {
        yield return new WaitForSeconds(2f);

        // Flicker glow
        screenOverlay.color = new Color(0, 0, 0, 1);
        for (int i = 0; i < 2; i++)
        {
            screenOverlay.color = new Color(0, 0.2f, 0, 0.8f);
            yield return new WaitForSeconds(0.2f);
            screenOverlay.color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(0.2f);
        }

        // Fade out overlay
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            screenOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Show terminal panel
        yield return new WaitForSeconds(2f);
        terminalPanel.SetActive(true);

        // Start terminal typing
        StartCoroutine(terminalIntro.RunIntroSequence());
    }
}


