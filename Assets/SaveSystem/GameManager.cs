using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public int damage;
    public bool isCollected; // Ensures the weapon must be picked up first
    public int upgradeCost; // Cost to upgrade weapon
    public int damageIncrease; // How much damage increases per upgrade
    public string weaponIconName; // Store sprite name
    [NonSerialized] public Sprite weaponIcon; // This will not be saved in JSON
    public int upgradeCount; // Track the number of upgrades
    public int maxUpgrades = 5; // Maximum number of upgrades allowed

    public WeaponData(string name, int dmg, int cost, int increase, string iconName = "")
    {
        weaponName = name;
        damage = dmg;
        isCollected = false;
        upgradeCost = cost;
        damageIncrease = increase;
        weaponIconName = iconName;
        weaponIcon = null; // Will be assigned after loading
        upgradeCount = 0; // Initialize upgrade count
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<WeaponData> collectedWeapons = new List<WeaponData>();
    public Vector3 savedPosition; // Store loaded position
    private bool shouldMovePlayer = false; // Flag to move player in LateUpdate
    public List<string> collectedItems = new List<string>();
    public List<string> defeatedBosses = new List<string>();

    public int playermaxHP = 100;
    public int playercurHP = 100;
    public int playerXP = 0;
    public int maxXP = 100;
    public int playerLevel = 1;
    public int upgradeOrb = 0;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set the save path when the game starts
            savePath = Application.persistentDataPath + "/savegame.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //exp counter
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
    //exp counter

    //weapon saver
    public void CollectWeapon(string weaponName, int damage, int upgradeCost, int damageIncrease, Sprite icon = null)
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

        // Store the sprite name instead of the sprite itself
        string iconName = icon != null ? icon.name : "";

        // If not found, add a new weapon to the collection
        WeaponData newWeapon = new WeaponData(weaponName, damage, upgradeCost, damageIncrease, iconName);
        newWeapon.isCollected = true;

        // Dynamically load the sprite if available
        if (!string.IsNullOrEmpty(iconName))
        {
            newWeapon.weaponIcon = Resources.Load<Sprite>("WeaponIcons/" + iconName);
        }

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
    //weapon saver

    //upgade orb counter
    public void GainUpgrade(int upgrade)
    {
        upgradeOrb += upgrade;
    }
    //upgade orb counter

    public void ResetGameData()
    {
        playermaxHP = 100;
        playercurHP = playermaxHP;
        playerXP = 0;
        maxXP = 100;
        playerLevel = 1;
        upgradeOrb = 0;
        collectedWeapons.Clear();
        collectedItems.Clear();
        defeatedBosses.Clear();
        savedPosition = Vector3.zero;
        SaveSystem.DeleteSave();
    }

    public void SaveGame()
    {
        if (string.IsNullOrEmpty(savePath))  // Prevents the null error
        {
            Debug.LogError("Save path is NULL! Cannot save game.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("GreyPlayer");
        if (player == null)
        {
            Debug.LogError("Player not found! Cannot save position.");
            return;
        }

        // Check if ObjectiveManager.Instance is null
        if (ObjectiveManager.Instance == null)
        {
            Debug.LogError("ObjectiveManager.Instance is null! Ensure ObjectiveManager is initialized.");
            return;
        }

        SaveData data = new SaveData
        {
            savedScene = SceneManager.GetActiveScene().name,
            playerHPmax = playermaxHP,
            playerHPcur = playercurHP,
            playerXP = playerXP,
            maxXP = maxXP,
            playerLevel = playerLevel,
            upgradeOrb = upgradeOrb,
            collectedWeapons = collectedWeapons,
            collectedItems = collectedItems,
            defeatedBosses = defeatedBosses,
            savedPosition = player.transform.position, // Save player position


            // Save ObjectiveManager state
            hasKey2 = ObjectiveManager.Instance.hasKey2,
            hasKey3 = ObjectiveManager.Instance.hasKey3
        };


        Debug.Log("Saved Position: " + data.savedPosition);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved at " + savePath);
    }

    public void LoadGame()
    {
        Time.timeScale = 1; // true time

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playermaxHP = data.playerHPmax;
            playercurHP = data.playerHPcur;
            playerXP = data.playerXP;
            maxXP = data.maxXP;
            playerLevel = data.playerLevel;
            upgradeOrb = data.upgradeOrb;
            collectedWeapons = data.collectedWeapons;
            collectedItems = data.collectedItems;
            defeatedBosses = data.defeatedBosses;
            savedPosition = data.savedPosition; // Store position

            // Load ObjectiveManager state
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.hasKey2 = data.hasKey2;
                ObjectiveManager.Instance.hasKey3 = data.hasKey3;

                // Notify the ObjectiveManager to update the objective text
                ObjectiveManager.Instance.UpdateObjective();
            }
            else
            {
                Debug.LogError("ObjectiveManager.Instance is null during LoadGame!");
            }

            // Reassign Sprites from Resources
            foreach (WeaponData weapon in collectedWeapons)
            {
                if (!string.IsNullOrEmpty(weapon.weaponIconName))
                {
                    weapon.weaponIcon = Resources.Load<Sprite>("WeaponIcons/" + weapon.weaponIconName);
                }
            }

            Debug.Log("Loading Game...");

            StartCoroutine(LoadSceneAndMove(data.savedScene));

            Debug.Log("Loaded Position: " + data.savedPosition);

            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.LogError("No save file found!");
        }
    }

    private IEnumerator LoadSceneAndMove(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Ensures scene objects exist
        GameObject player = GameObject.FindGameObjectWithTag("GreyPlayer");

        if (player == null)
        {
            Debug.Log("No player found. Spawning at saved position...");

            Debug.Log("Saved Position: " + savedPosition);

            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.SpawnPlayer(savedPosition);
            }
            else
            {
                Debug.LogError("SpawnManager not found in scene!");
            }
        }
        else
        {
            Debug.Log("Before applying position: " + player.transform.position);
            Debug.Log("Loaded Position: " + savedPosition);
            player.transform.position = savedPosition;
            Debug.Log("Loaded Player Position: " + savedPosition);
        }
    }

    public bool SaveExists()
    {
        return File.Exists(savePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }

    public void CollectItem(string itemName)
    {
        if (!collectedItems.Contains(itemName))
        {
            collectedItems.Add(itemName);
            Debug.Log(itemName + " has been collected!");
        }
    }

    public bool HasItem(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    public void BossDefeated(string bossName)
    {
        defeatedBosses.Add(bossName);
        Debug.Log(bossName + " is been added and defeated.");
    }
}