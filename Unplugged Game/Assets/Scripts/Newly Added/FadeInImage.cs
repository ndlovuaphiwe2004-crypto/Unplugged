using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInImage : MonoBehaviour
{
    public Image backgroundImage;   
    public float fadeDuration = 2f; 

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color imgColor = backgroundImage.color;
        imgColor.a = 0f;
        backgroundImage.color = imgColor;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            imgColor.a = Mathf.Clamp01(elapsed / fadeDuration);
            backgroundImage.color = imgColor;
            yield return null;
        }
    }
}
