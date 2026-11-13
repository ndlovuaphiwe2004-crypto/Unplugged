using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource backgroundMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persists across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure backgroundMusic is always linked to the AudioSource on this object
        if (backgroundMusic == null)
        {
            backgroundMusic = GetComponent<AudioSource>();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Re-verify reference when any scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (backgroundMusic == null)
        {
            backgroundMusic = GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
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