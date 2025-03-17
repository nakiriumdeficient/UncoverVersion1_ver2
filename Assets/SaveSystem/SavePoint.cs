using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    public GameObject upgradeUI;     // UI panel for upgrades
    public GameObject savePromptUI;

    private bool playerInRange = false;
    private bool upgradeMenuOpen = false;

    public Transform weaponListParent; // Parent object for weapon buttons
    public GameObject weaponButtonPrefab; // Prefab for weapon buttons
    public TextMeshProUGUI orbText; // UI text to show upgrade orbs

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
            GameManager.Instance.SaveGame();
            Debug.Log("Game Saved!");
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
    }
    void OpenUpgradeMenu()
    {
        Debug.Log("Opening Upgrade Menu..."); // Debugging message
        upgradeUI.SetActive(true);
        upgradeMenuOpen = true;

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
    }
}
