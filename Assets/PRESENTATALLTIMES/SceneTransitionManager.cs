using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private string targetScene;
    private string entryPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToScene(string sceneName, string spawnPointName)
    {
        targetScene = sceneName;
        entryPoint = spawnPointName;
        GameManager.Instance.savedPosition = Vector3.zero; // Reset saved position
        SceneManager.LoadScene(sceneName);
        StartCoroutine(HandleSceneLoaded());
    }

    private IEnumerator HandleSceneLoaded()
    {
        yield return new WaitForSeconds(0.5f); // Allow time for scene to load

        // Find the correct spawn point
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        bool spawnFound = false;

        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.spawnPointName == entryPoint)
            {
                GameManager.Instance.savedPosition = sp.transform.position;
                spawnFound = true;
                Debug.Log("Spawn point found! Position: " + sp.transform.position);
                break;
            }
        }
        if (!spawnFound)
        {
            Debug.LogError("No matching spawn point found! Defaulting to (0, 0, 0)");
            GameManager.Instance.savedPosition = Vector3.zero;
        }

        // Spawn player at the correct position
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.SpawnPlayer(GameManager.Instance.savedPosition);
        }
        else
        {
            Debug.LogError("SpawnManager not found in scene!");
        }
    }
}
