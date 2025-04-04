using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI; // Assign Inventory Panel in the Inspector
    public Transform itemListParent; // Parent for dynamically added items
    public GameObject itemPrefab; // UI prefab for listing items
    public TextMeshProUGUI itemDescriptionText; // Text to show item details

    private List<GameObject> spawnedItems = new List<GameObject>();
    private GreyController playerController; // Reference to player movement script
    private GreyAttack playerAttack;
    void Start()
    {
        inventoryUI.SetActive(false); // Hide inventory at start
        playerController = FindObjectOfType<GreyController>(); // Find player script
        playerAttack = FindObjectOfType<GreyAttack>(); // Find player script
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Press 'I' to toggle inventory
        {
            ToggleInventory();
        }
    }
    private int GetMaxWeaponUpgrades()
    {
        return GameManager.Instance.playerLevel / 2 + 2; // Increases every 2 levels
    }
    public void ToggleInventory()
    {

        bool isActive = inventoryUI.activeSelf;



        inventoryUI.SetActive(!isActive); // Toggle Inventory UI

        if (playerController != null)
        {
            playerController.enabled = isActive; // Disable movement when inventory is open
        }

        if (!isActive)
        {
            PopulateInventory();
            Time.timeScale = 0; // Pause game when inventory is open
        }
        else
        {
            Time.timeScale = 1; // Resume game when inventory is closed
        }
    }

    void PopulateInventory()
    {
        // Clear previous items
        foreach (GameObject item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();

        // Load collected weapons
        foreach (WeaponData weapon in GameManager.Instance.collectedWeapons)
        {
            GameObject newItem = Instantiate(itemPrefab, itemListParent);
            newItem.GetComponentInChildren<TextMeshProUGUI>().text = weapon.weaponName;
            newItem.GetComponent<Button>().onClick.AddListener(() => ShowWeaponDetails(weapon));
            spawnedItems.Add(newItem);
        }

        // Load collected items
        foreach (string item in GameManager.Instance.collectedItems)
        {
            GameObject newItem = Instantiate(itemPrefab, itemListParent);
            newItem.GetComponentInChildren<TextMeshProUGUI>().text = item;
            newItem.GetComponent<Button>().onClick.AddListener(() => ShowItemDetails(item));
            spawnedItems.Add(newItem);
        }
    }

    void ShowWeaponDetails(WeaponData weapon)
    {
        int maxUpgrades = GetMaxWeaponUpgrades(); // Get dynamic max upgrades
        itemDescriptionText.text = $"{weapon.weaponName}\nDamage: {weapon.damage}\nUpgrades: {weapon.upgradeCount}/{maxUpgrades}";
    }

    void ShowItemDetails(string itemName)
    {
        itemDescriptionText.text = $"{itemName}\nThis is a collected item.";
    }
}