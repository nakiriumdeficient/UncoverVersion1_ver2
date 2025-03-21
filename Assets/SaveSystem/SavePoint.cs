using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    public GameObject upgradeUI;     // UI panel for upgrades
    public GameObject savePromptUI;
    public TextMeshProUGUI saveMessageUI; // Reference to the "Save Successful!" UI
    public GameObject mapImage; // Reference to the map UI image

    private bool playerInRange = false;
    private bool upgradeMenuOpen = false;
    private bool mapOpen = false; // Track if the map is open

    public Transform weaponListParent; // Parent object for weapon buttons
    public GameObject weaponButtonPrefab; // Prefab for weapon buttons
    public TextMeshProUGUI orbText; // UI text to show upgrade orbs

    private void Start()
    {
        if (saveMessageUI != null)
        {
            saveMessageUI.gameObject.SetActive(false); // Hide message at start
        }

        if (mapImage != null)
        {
            mapImage.SetActive(false); // Ensure the map is hidden at start
        }
    }

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
            upgradeUI.SetActive(false); // Close upgrade menu when leaving
            upgradeMenuOpen = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.playercurHP = GameManager.Instance.playermaxHP;
            GameManager.Instance.SaveGame();
            Debug.Log("Game Saved!");
            StartCoroutine(ShowSaveMessage());
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R Pressed!"); // Debugging message
            if (!upgradeMenuOpen)
            {
                OpenUpgradeMenu();
            }
            else
            {
                CloseUpgradeMenu();
            }
        }

        // Toggle map visibility when M is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    private IEnumerator ShowSaveMessage()
    {
        if (saveMessageUI != null)
        {
            saveMessageUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f); // Show for 2 seconds
            saveMessageUI.gameObject.SetActive(false);
        }
    }

    void OpenUpgradeMenu()
    {
        Debug.Log("Opening Upgrade Menu..."); // Debugging message
        upgradeUI.SetActive(true);
        upgradeMenuOpen = true;
        Time.timeScale = 0; // Freeze game

        // Update upgrade orbs text
        orbText.text = "Upgrade Orbs: " + GameManager.Instance.upgradeOrb;

        // Clear previous buttons
        foreach (Transform child in weaponListParent)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for collected weapons
        foreach (WeaponData weapon in GameManager.Instance.collectedWeapons)
        {
            if (weapon.isCollected)
            {
                GameObject buttonObj = Instantiate(weaponButtonPrefab, weaponListParent);
                RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(300, 200); // Set a reasonable size
                rectTransform.localScale = Vector3.one; // Reset scale to prevent shrinking

                WeaponButton button = buttonObj.GetComponent<WeaponButton>();
                button.Setup(weapon);
            }
        }
    }

    void CloseUpgradeMenu()
    {
        upgradeUI.SetActive(false);
        upgradeMenuOpen = false;
        Time.timeScale = 1; // Resume game
        // Destroy weapon buttons so they reset on next open
        foreach (Transform child in weaponListParent)
        {
            Destroy(child.gameObject);
        }
    }

    void ToggleMap()
    {
        if (mapImage != null)
        {
            mapOpen = !mapOpen; // Toggle the map state
            mapImage.SetActive(mapOpen); // Show/hide the map
            Time.timeScale = mapOpen ? 0 : 1; // Pause/unpause the game
        }
        else
        {
            Debug.LogError("Map Image is not assigned in the SavePoint script!");
        }
    }
}