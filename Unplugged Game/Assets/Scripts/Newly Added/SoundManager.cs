using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource backgroundMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    public void Mute()
    {
        if (backgroundMusic != null)
            backgroundMusic.mute = true;
    }

    public void Unmute()
    {
        if (backgroundMusic != null)
            backgroundMusic.mute = false;
    }
}
