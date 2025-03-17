using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.UI;

public class WeaponUpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel; // Reference to the upgrade UI Panel
    public Button[] upgradeButtons; // Buttons for upgrading weapons
    public TextMeshProUGUI[] weaponTexts; // TextMeshPro elements for weapon names and upgrade levels
    public TextMeshProUGUI orbsText; // TextMeshPro element for orbs
    public TextMeshProUGUI goldText; // TextMeshPro element for gold

    private int[] upgrades = new int[3]; // Track upgrades for Falx, Shatterack, Hasta
    private int orbs = 0; // Collected orbs
    private int gold = 0; // Collected gold

    // Upgrade costs and damage increases
    private readonly int[] upgradeOrbCosts = { 5, 10, 15, 20, 25 };
    private readonly int[] upgradeGoldCosts = { 10, 20, 30, 40, 50 };
    private readonly int[] upgradeDamageIncreases = { 10, 15, 15, 20, 35 };

    void Start()
    {
        // Hide the UI at the start
        upgradePanel.SetActive(false);

        // Add listeners to the upgrade buttons
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i; // Capture the index for the listener
            upgradeButtons[i].onClick.AddListener(() => UpgradeWeapon(index));
        }

        // Initialize UI
        UpdateUI();
    }

    void Update()
    {
        // Toggle UI visibility when 'B' is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            upgradePanel.SetActive(!upgradePanel.activeSelf);
            UpdateUI(); // Refresh UI when opened
        }

        // Go back to the previous UI when 'Back' is pressed (placeholder for now)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            upgradePanel.SetActive(false);
        }
    }

    void UpgradeWeapon(int weaponIndex)
    {
        if (upgrades[weaponIndex] >= 5)
        {
            Debug.Log("Weapon is already fully upgraded!");
            return;
        }

        int requiredOrbs = upgradeOrbCosts[upgrades[weaponIndex]];
        int requiredGold = upgradeGoldCosts[upgrades[weaponIndex]];

        if (orbs >= requiredOrbs && gold >= requiredGold)
        {
            // Deduct orbs and gold
            orbs -= requiredOrbs;
            gold -= requiredGold;

            // Apply upgrade
            upgrades[weaponIndex]++;
            UpdateUI();

            Debug.Log($"Upgraded {GetWeaponName(weaponIndex)} to level {upgrades[weaponIndex]}!");
        }
        else
        {
            Debug.Log("Not enough orbs or gold to upgrade!");
        }
    }

    void UpdateUI()
    {
        // Update weapon texts
        for (int i = 0; i < weaponTexts.Length; i++)
        {
            weaponTexts[i].text = $"{GetWeaponName(i)} +{upgrades[i]}";
        }

        // Update orbs and gold display
        orbsText.text = $"Orbs: {orbs}";
        goldText.text = $"Gold: {gold}";
    }

    string GetWeaponName(int index)
    {
        switch (index)
        {
            case 0: return "Falx";
            case 1: return "Shatterack";
            case 2: return "Hasta";
            default: return "Unknown";
        }
    }

    // Call this method to add orbs (e.g., when collected in the game)
    public void AddOrbs(int amount)
    {
        orbs += amount;
        UpdateUI();
    }

    // Call this method to add gold (e.g., when collected in the game)
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    // Get the total damage increase for a weapon
    public int GetDamageIncrease(int weaponIndex)
    {
        int totalDamage = 0;
        for (int i = 0; i < upgrades[weaponIndex]; i++)
        {
            totalDamage += upgradeDamageIncreases[i];
        }
        return totalDamage;
    }
}