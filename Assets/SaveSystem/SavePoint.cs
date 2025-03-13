using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    public GameObject upgradeUI;
    public GameObject savePromptUI;
    private bool playerInRange = false;
    public bool upgradeExit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // Ensure only the player can save
        {
            playerInRange = true;
            savePromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            playerInRange = false;
            savePromptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.SaveGame();
            Debug.Log("Game Saved!");
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            upgradeUI.SetActive(true);
        }
    }
}
