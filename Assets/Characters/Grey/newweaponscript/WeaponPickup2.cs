using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup2 : MonoBehaviour
{
    public string weaponName;
    public int damage;
    public int upgradeCost; // Cost to upgrade weapon
    public int damageIncrease; // How much damage increases per upgrade
    public Sprite weaponIcon; // Weapon icon for UI

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // Ensure only player can pick up
        {
            GameManager.Instance.CollectWeapon(weaponName, damage, upgradeCost, damageIncrease, weaponIcon);
            GameObject.FindObjectOfType<HotBarUI>().UpdateHotbar();
            Debug.Log("Picked up: " + weaponName);

            Destroy(gameObject); // Remove from scene
        }
    }
}
