using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExitTriggerS : MonoBehaviour
{
    public string targetScene;
    public string targetSpawnPoint;
    public string requiredItem; // The key or item required to transition

    private bool playerIsNear = false; // Tracks if player is in range
    private GameObject ExitPrompt; // Reference to UI Prompt
    private void Start()
    {
        // Find the UI Prompt in the scene (Make sure it's named "ExitPrompt")
        ExitPrompt = GameObject.Find("ExitPrompt");

        if (ExitPrompt != null)
        {
            ExitPrompt.SetActive(false); // Hide UI at start
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            playerIsNear = true;

            // Show prompt if found
            if (ExitPrompt != null)
            {
                ExitPrompt.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            playerIsNear = false;

            // Hide prompt if found
            if (ExitPrompt != null)
            {
                ExitPrompt.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.S)) // Press S to transition
        {
            if (!string.IsNullOrEmpty(requiredItem) && !GameManager.Instance.HasItem(requiredItem))
            {
                Debug.Log("You need the " + requiredItem + " to pass!");
                return;
            }

            // Hide prompt before transitioning
            if (ExitPrompt != null)
            {
                ExitPrompt.SetActive(false);
            }

            SceneTransitionManager.Instance.TransitionToScene(targetScene, targetSpawnPoint);
        }
    }
}
