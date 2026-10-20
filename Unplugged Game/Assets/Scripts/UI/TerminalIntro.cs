using UnityEngine;
using TMPro;
using System.Collections;

public class TerminalIntro : MonoBehaviour
{
    public CanvasGroup terminalPanelGroup;
    public TMP_Text terminalText;
    [TextArea] public string introText;
    public float typeSpeed = 0.05f;

    public AudioSource typewriterAudio;

    // Slice settings (adjust in Inspector)
    public float sliceStart = 0f;
    public float sliceEnd = 1f;

    public IEnumerator RunIntroSequence()
    {
        // Boot messages
        yield return StartCoroutine(TypeLineBoot("Initializing system...", 0.07f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeLineBoot("Loading modules...", 0.07f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeLineBoot("Access granted.", 0.07f));
        yield return new WaitForSeconds(1f);

        terminalText.text = "";

        // Fade in panel
        terminalPanelGroup.alpha = 0;
        while (terminalPanelGroup.alpha < 1)
        {
            terminalPanelGroup.alpha += Time.deltaTime * 1.5f;
            yield return null;
        }

        // Story text
        yield return StartCoroutine(TypeLineWithCursor(introText, typeSpeed));
        StartCoroutine(BlinkCursor()); // permanent blink at end
    }

    // Boot messages typing (simple, but with punctuation pauses)
    IEnumerator TypeLineBoot(string line, float speed)
    {
        terminalText.text += "\n";
        foreach (char c in line)
        {
            terminalText.text += c;
            PlayTypewriterSoundSlice(sliceStart, sliceEnd, speed);

            float delay = speed;
            if (c == '.' || c == ':' || c == ';') delay += 0.4f;
            else if (c == ',') delay += 0.15f;

            yield return new WaitForSeconds(delay);
        }
    }

    // Story typing with underscore cursor, punctuation pauses, and blinking during pauses
    IEnumerator TypeLineWithCursor(string line, float speed)
    {
        terminalText.text += "\n";
        foreach (char c in line)
        {
            terminalText.text += c + "_";
            PlayTypewriterSoundSlice(sliceStart, sliceEnd, speed);

            float delay = speed;
            bool isPause = false;

            if (c == '.' || c == ':' || c == ';')
            {
                delay += 0.4f;
                isPause = true;
            }
            else if (c == ',')
            {
                delay += 0.15f;
                isPause = true;
            }

            if (isPause)
            {
                Coroutine blink = StartCoroutine(BlinkCursorTemp(delay));
                yield return new WaitForSeconds(delay);
                StopCoroutine(blink);
                terminalText.text = terminalText.text.TrimEnd('_');
            }
            else
            {
                yield return new WaitForSeconds(delay);
                terminalText.text = terminalText.text.TrimEnd('_');
            }
        }
    }

    // Permanent blinking cursor at the end
    IEnumerator BlinkCursor()
    {
        while (true)
        {
            terminalText.text += "_";
            yield return new WaitForSeconds(0.5f);
            terminalText.text = terminalText.text.TrimEnd('_');
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Temporary blinking cursor during punctuation pauses
    IEnumerator BlinkCursorTemp(float pauseDuration)
    {
        float elapsed = 0f;
        while (elapsed < pauseDuration)
        {
            terminalText.text += "_";
            yield return new WaitForSeconds(0.5f);
            terminalText.text = terminalText.text.TrimEnd('_');
            yield return new WaitForSeconds(0.5f);
            elapsed += 1f; // each cycle ~1 second
        }
    }

    // Plays only a slice of the clip per keystroke
    void PlayTypewriterSoundSlice(float startTime, float endTime, float charSpeed)
    {
        if (typewriterAudio != null && typewriterAudio.clip != null)
        {
            typewriterAudio.Stop();
            typewriterAudio.time = startTime;
            typewriterAudio.pitch = Random.Range(0.95f, 1.05f);
            typewriterAudio.Play();

            float sliceDuration = endTime - startTime;
            float duration = Mathf.Min(sliceDuration, charSpeed * 0.9f);
            StartCoroutine(StopSoundEarly(duration));
        }
    }

    IEnumerator StopSoundEarly(float duration)
    {
        yield return new WaitForSeconds(duration);
        typewriterAudio.Stop();
    }
}
