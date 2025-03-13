using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public int damage;
    public bool isCollected; // Ensures the weapon must be picked up first

    public WeaponData(string name, int dmg)
    {
        weaponName = name;
        damage = dmg;
        isCollected = false;
    }
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<WeaponData> collectedWeapons = new List<WeaponData>();

    

    public int playermaxHP = 100;
    public int playercurHP = 100;
    public int playerXP = 0;
    public int playerLevel = 1;
    public int maxXP = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GainXP(int xp)
    {
        playerXP += xp;

        if (playerXP >= maxXP)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        playerXP -= maxXP; // Carry over excess XP
        playerLevel++;
        maxXP += 50; // Increase XP required for next level
    }
    public void CollectWeapon(string weaponName, int damage)
    {
        // Check if weapon is already in the collection
        foreach (WeaponData weapon in collectedWeapons)
        {
            if (weapon.weaponName == weaponName)
            {
                weapon.isCollected = true; // Mark as collected
                return;
            }
        }

        // If not found, add a new weapon to the collection
        WeaponData newWeapon = new WeaponData(weaponName, damage);
        newWeapon.isCollected = true;
        collectedWeapons.Add(newWeapon);
    }
    public WeaponData GetWeapon(string weaponName)
    {
        foreach (WeaponData weapon in collectedWeapons)
        {
            if (weapon.weaponName == weaponName && weapon.isCollected)
            {
                return weapon;
            }
        }
        return null; // Weapon not found or not collected
    }
}
