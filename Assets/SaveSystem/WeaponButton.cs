using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponDamageText;
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
        upgradeButton.onClick.AddListener(UpgradeWeapon);
        Debug.Log("Button created for: " + weapon.weaponName); // Debugging message

    }

    void UpgradeWeapon()
    {
        if (GameManager.Instance.upgradeOrb > 0)
        {
            GameManager.Instance.upgradeOrb--; // Spend an upgrade orb
            weaponData.damage += 5; // Increase weapon damage
            weaponDamageText.text = "Damage: " + weaponData.damage;

            // Update orb count in UI
            FindObjectOfType<SavePoint>().orbText.text = "Upgrade Orbs: " + GameManager.Instance.upgradeOrb;

            Debug.Log(weaponData.weaponName + " upgraded! New Damage: " + weaponData.damage);
        }
        else
        {
            Debug.Log("Not enough upgrade orbs!");
        }
    }
}
