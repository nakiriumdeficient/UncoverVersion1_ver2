using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExitTrigger : MonoBehaviour
{
    public string targetScene;
    public string targetSpawnPoint;
    public string requiredItem; // The key or item required to transition

    private bool playerIsNear = false; // Tracks if player is in range
    private GameObject exitPrompt; // Reference to UI Prompt

    private void Start()
    {
        // Find ExitPrompt anywhere in the scene
        exitPrompt = GameObject.FindObjectOfType<Canvas>().transform.Find("ExitPrompt")?.gameObject;


        if (exitPrompt != null)
        {
            exitPrompt.SetActive(false); // Ensure it's off at start
        }
        else
        {
            Debug.LogError("ExitPrompt not found! Make sure it's named 'ExitPrompt' in the Canvas.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            playerIsNear = true;

            exitPrompt?.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            playerIsNear = false;

            // Hide prompt if found
            if (exitPrompt != null)
            {
                exitPrompt.SetActive(false);
                Debug.Log("ExitPrompt disabled");
            }
        }
    }
    private void Update()

    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.W)) // Press W to transition
        {
            if (!string.IsNullOrEmpty(requiredItem) && !GameManager.Instance.HasItem(requiredItem))
            {
                Debug.Log("You need the " + requiredItem + " to pass!");
                return;
            }

            // Hide prompt before transitioning
            if (exitPrompt != null)
            {
                exitPrompt.SetActive(false);
            }

            SceneTransitionManager.Instance.TransitionToScene(targetScene, targetSpawnPoint);
        }
    }
}
