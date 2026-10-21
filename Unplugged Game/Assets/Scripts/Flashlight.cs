using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
    private Light myLight;

    public float flashInterval = 0.5f;
    public float onIntensity = 3f;

    void Start()
    {
        myLight = GetComponent<Light>();

        if (myLight == null)
        {
            Debug.LogError("Light component not found!");
            return;
        }

        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            myLight.intensity = onIntensity;
            yield return new WaitForSeconds(flashInterval);

            myLight.intensity = 0f;
            yield return new WaitForSeconds(flashInterval);
        }
    }
}