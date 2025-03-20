using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip UIbgm;
    public AudioClip bgm1; // Assign BGM1 (Levels 1-11)
    public AudioClip bgm2; // Assign BGM2 (Levels 12-27)
    public AudioClip bgm3; // Assign BGM3 (Levels 28-36)

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        // Ensure BGM starts playing at the beginning
        PlayBGMForScene();
    }

    void Start()
    {
        PlayBGMForScene();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene();
    }

    void PlayBGMForScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip newBGM = GetBGMForScene(sceneName);

        if (newBGM != null && audioSource.clip != newBGM)
        {
            audioSource.clip = newBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    AudioClip GetBGMForScene(string sceneName)
    {
        // Check for Main Menu
        if (sceneName == "UIScene") // Change this if your main menu scene has a different name
        {
            return UIbgm;
        }
        // Extract number from scene name (e.g., "Level1" -> 1)
        if (sceneName.StartsWith("Level"))
        {
            string numberPart = sceneName.Substring(5); // Remove "Level"
            if (int.TryParse(numberPart, out int levelNumber))
            {
                if (levelNumber >= 1 && levelNumber <= 11)
                {
                    return bgm1;
                }
                else if (levelNumber >= 12 && levelNumber <= 27)
                {
                    return bgm2;
                }
                else if (levelNumber >= 28 && levelNumber <= 36)
                {
                    return bgm3;
                }
            }
        }

        return null; // Default to no music if scene name doesn't match pattern
    }
}
