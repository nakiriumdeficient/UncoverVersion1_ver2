using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    public GameObject savePromptUI;
    private bool playerInRange = false;
    private PlayerStats playerStats;

    void Start()
    {
        savePromptUI.SetActive(false);
        playerStats = GameObject.FindWithTag("GreyPlayer").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SaveSystem.SaveGame(
                playerStats.transform.position,
                playerStats.level,
                playerStats.experience
            );

            savePromptUI.GetComponentInChildren<UnityEngine.UI.Text>().text = "Game Saved!";
            Invoke("HidePrompt", 1.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            playerInRange = true;
            savePromptUI.SetActive(true);
            savePromptUI.GetComponentInChildren<UnityEngine.UI.Text>().text = "Press [E] to Save";
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

    private void HidePrompt()
    {
        savePromptUI.SetActive(false);
    }
}
