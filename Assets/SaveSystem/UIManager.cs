using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

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
        // Load the UI Scene if it's not already loaded
        if (SceneManager.GetSceneByName("UI") == null)
        {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }
}
