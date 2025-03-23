using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponDamageText;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI damageIncreaseText;
    public Image weaponIconImage;
    public Button upgradeButton;

    private WeaponData weaponData;

    public AudioSource upgradeSound;
    private bool isCooldown = false;

    void Start()
    {
        UpdateButtonState();
    }

    void Update()
    {
        // Update logic (if needed)
    }

    public void Setup(WeaponData weapon)
    {
        weaponData = weapon;
        weaponNameText.text = weapon.weaponName;
        weaponDamageText.text = "Damage: " + weapon.damage;
        upgradeCostText.text = "Upgrade Cost: " + weaponData.upgradeCost + " Orbs";
        damageIncreaseText.text = "Damage + " + weaponData.damageIncrease;

        if (weapon.weaponIcon != null)
        {
            weaponIconImage.sprite = weapon.weaponIcon;
        }

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => UpgradeWeapon());
        Debug.Log("Button created for: " + weapon.weaponName);

        UpdateButtonState();
    }

    void UpgradeWeapon()
    {
        // Check if the weapon has reached the maximum number of upgrades
        if (weaponData.upgradeCount >= weaponData.maxUpgrades)
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

            // Increment the upgrade count
            weaponData.upgradeCount++;

            // Update the UI
            weaponDamageText.text = "Damage: " + weaponData.damage;
            upgradeCostText.text = "Upgrade Cost: " + weaponData.upgradeCost + " Orbs";
            damageIncreaseText.text = "Damage + " + weaponData.damageIncrease;

            // Update the upgrade orb UI
            FindObjectOfType<SavePoint>().orbText.text = "Upgrade Orbs: " + GameManager.Instance.upgradeOrb;

            // Play the upgrade sound
            if (upgradeSound != null)
            {
                upgradeSound.Play();
            }

            // Debugging message
            Debug.Log(weaponData.weaponName + " upgraded! New Damage: " + weaponData.damage + " | Upgrades: " + weaponData.upgradeCount + "/" + weaponData.maxUpgrades);

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
        if (weaponData.upgradeCount >= weaponData.maxUpgrades)
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