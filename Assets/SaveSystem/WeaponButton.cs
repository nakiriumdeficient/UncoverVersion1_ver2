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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        upgradeButton.onClick.AddListener(UpgradeWeapon);
        Debug.Log("Button created for: " + weapon.weaponName); // Debugging message

    }

    void UpgradeWeapon()
    {
        if (GameManager.Instance.upgradeOrb >= weaponData.upgradeCost)
        {
            GameManager.Instance.upgradeOrb -= weaponData.upgradeCost; // Spend upgrade orbs
            weaponData.damage += weaponData.damageIncrease; // Increase weapon damage

            weaponDamageText.text = "Damage: " + weaponData.damage;
            upgradeCostText.text = "Upgrade Cost: " + weaponData.upgradeCost + " Orbs";
            damageIncreaseText.text = "Damage + " + weaponData.damageIncrease;

            FindObjectOfType<SavePoint>().orbText.text = "Upgrade Orbs: " + GameManager.Instance.upgradeOrb;

            Debug.Log(weaponData.weaponName + " upgraded! New Damage: " + weaponData.damage);
        }
        else
        {
            Debug.Log("Not enough upgrade orbs!");
        }
    }
}
