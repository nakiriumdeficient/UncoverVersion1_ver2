using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
    public Vector3 savedPosition; // Store loaded position
    private bool shouldMovePlayer = false; // Flag to move player in LateUpdate

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
    //weapon saver

    //upgade orb counter
    public void GainUpgrade(int upgrade)
    {
        upgradeOrb += upgrade;
    }
    //upgade orb counter
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
            savedPosition = GameObject.FindGameObjectWithTag("GreyPlayer").transform.position // Save player position
        };
        Debug.Log("Saved Position: " + data.savedPosition);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved at " + savePath);
    }

    public void LoadGame()
    {
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

            SceneManager.LoadScene(data.savedScene);

            Debug.Log("Loading Game...");

            savedPosition = data.savedPosition; // Store position
            shouldMovePlayer = true; // Enable movement after load

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

        yield return new WaitForSeconds(0.5f); // Ensures scene objects exist
    }
    private void LateUpdate()
    {
        if (shouldMovePlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("GreyPlayer");
            if (player != null)
            {
                player.transform.position = savedPosition;
                Debug.Log("LateUpdate forced player move: " + savedPosition);
                shouldMovePlayer = false; // Prevent running again
            }
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
}

