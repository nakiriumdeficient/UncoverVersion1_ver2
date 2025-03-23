using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponDamageText;
    public TextMeshProUGUI upgradeCostText; // New UI element for upgrade cost
    public TextMeshProUGUI damageIncreaseText; // New UI element for damage increase
    public Image weaponIconImage;
    public Button upgradeButton;

    private WeaponData weaponData;
    private int upgradeCount = 0; // Track the number of upgrades
    private const int maxUpgrades = 5; // Maximum number of upgrades allowed

    public AudioSource upgradeSound; // Sound effect for upgrading
    private bool isCooldown = false; // Cooldown flag

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the button state
        UpdateButtonState();
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic (if needed)
    }

    public void Setup(WeaponData weapon)
    {
        weaponData = weapon;
        weaponNameText.text = weapon.weaponName;
        weaponDamageText.text = "Damage: " + weapon.damage;
        upgradeCostText.text = "Upgrade Cost: " + weaponData.upgradeCost + " Orbs"; // Show upgrade cost
        damageIncreaseText.text = "Damage + " + weaponData.damageIncrease; // Show how much damage will increase

        if (weapon.weaponIcon != null)
        {
            weaponIconImage.sprite = weapon.weaponIcon;
        }

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => UpgradeWeapon());
        Debug.Log("Button created for: " + weapon.weaponName); // Debugging message

        // Initialize the button state
        UpdateButtonState();
    }

    void UpgradeWeapon()
    {
        // Check if the weapon has reached the maximum number of upgrades
        if (upgradeCount >= maxUpgrades)
        {
            Debug.Log(weaponData.weaponName + " has reached the maximum number of upgrades!");
            return;
        }

        // Check if the player has enough upgrade orbs
        if (GameManager.Instance.upgradeOrb >= weaponData.upgradeCost)
        {
            // Spend upgrade orbs
            GameManager.Instance.upgradeOrb -= weaponData.upgradeCost;

            // Increase weapon damage
            weaponData.damage += weaponData.damageIncrease;

            // Update the UI
            weaponDamageText.text = "Damage: " + weaponData.damage;
            upgradeCostText.text = "Upgrade Cost: " + weaponData.upgradeCost + " Orbs";
            damageIncreaseText.text = "Damage + " + weaponData.damageIncrease;

            // Update the upgrade orb UI
            FindObjectOfType<SavePoint>().orbText.text = "Upgrade Orbs: " + GameManager.Instance.upgradeOrb;

            // Increment the upgrade count
            upgradeCount++;

            // Play the upgrade sound
            if (upgradeSound != null)
            {
                upgradeSound.Play();
            }

            // Debugging message
            Debug.Log(weaponData.weaponName + " upgraded! New Damage: " + weaponData.damage + " | Upgrades: " + upgradeCount + "/" + maxUpgrades);

            // Update the button state
            UpdateButtonState();
        }
        else
        {
            Debug.Log("Not enough upgrade orbs!");
        }
    }

    private void UpdateButtonState()
    {
        // Disable the upgrade button if the weapon has reached the maximum number of upgrades
        if (upgradeCount >= maxUpgrades)
        {
            upgradeButton.interactable = false;
            upgradeCostText.text = "Max Upgrades Reached!";
            damageIncreaseText.text = ""; // Clear the damage increase text
        }
        else
        {
            upgradeButton.interactable = true;
        }
    }
}