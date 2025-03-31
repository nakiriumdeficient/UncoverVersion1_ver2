using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InformationWeapon
{
    public string weaponName;
}

[System.Serializable]
public class InformationEnemy
{
    public string enemyName;
}

public class JournalManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject weaponsPage;
    public GameObject enemiesPage;
    public Button weaponsTabButton;
    public Button enemiesTabButton;

    [Header("Weapons Tab")]
    public InformationWeapon[] weaponsInformation;
    public Button weaponsNextButton;
    public Button weaponsPrevButton;
    public TMP_Text weaponNameText;
    public int currentWeaponIndex = 0;
    public bool weaponInputLock = false;

    [Header("Enemies Tab")]
    public InformationEnemy[] enemiesInformation;
    public Button enemiesNextButton;
    public Button enemiesPrevButton;
    public TMP_Text enemyNameText;
    public int currentEnemyIndex = 0;
    public bool enemyInputLock = false;

    public void Start()
    {
        // Clear any existing listeners to prevent duplicates
        weaponsNextButton.onClick.RemoveAllListeners();
        weaponsPrevButton.onClick.RemoveAllListeners();
        enemiesNextButton.onClick.RemoveAllListeners();
        enemiesPrevButton.onClick.RemoveAllListeners();
        weaponsTabButton.onClick.RemoveAllListeners();
        enemiesTabButton.onClick.RemoveAllListeners();

        // Set up single listeners
        weaponsNextButton.onClick.AddListener(() => SafeNextWeapon());
        weaponsPrevButton.onClick.AddListener(() => SafePreviousWeapon());
        enemiesNextButton.onClick.AddListener(() => SafeNextEnemy());
        enemiesPrevButton.onClick.AddListener(() => SafePreviousEnemy());
        weaponsTabButton.onClick.AddListener(ShowWeaponsTab);
        enemiesTabButton.onClick.AddListener(ShowEnemiesTab);

        // Debug initialization
        Debug.Log("[Journal] Initializing with:");
        DebugWeaponList();
        DebugEnemyList();

        // Start with weapons tab
        ShowWeaponsTab();
    }

    public void DebugWeaponList()
    {
        Debug.Log($"[Weapons] Loaded {weaponsInformation.Length} weapons:");
        for (int i = 0; i < weaponsInformation.Length; i++)
        {
            Debug.Log($"[Weapons] {i}: {weaponsInformation[i]?.weaponName ?? "NULL"}");
        }
    }

    public void DebugEnemyList()
    {
        Debug.Log($"[Enemies] Loaded {enemiesInformation.Length} enemies:");
        for (int i = 0; i < enemiesInformation.Length; i++)
        {
            Debug.Log($"[Enemies] {i}: {enemiesInformation[i]?.enemyName ?? "NULL"}");
        }
    }

    #region Tab Management
    public void ShowWeaponsTab()
    {
        weaponsPage.SetActive(true);
        enemiesPage.SetActive(false);
        UpdateWeaponDisplay();
        Debug.Log("[Tabs] Switched to Weapons tab");
    }

    public void ShowEnemiesTab()
    {
        weaponsPage.SetActive(false);
        enemiesPage.SetActive(true);
        UpdateEnemyDisplay();
        Debug.Log("[Tabs] Switched to Enemies tab");
    }
    #endregion

    #region Weapon System
    public void SafeNextWeapon()
    {
        if (!weaponInputLock && weaponsInformation.Length > 1)
        {
            weaponInputLock = true;
            NextWeapon();
            weaponInputLock = false;
        }
    }

    public void SafePreviousWeapon()
    {
        if (!weaponInputLock && weaponsInformation.Length > 1)
        {
            weaponInputLock = true;
            PreviousWeapon();
            weaponInputLock = false;
        }
    }

    public void NextWeapon()
    {
        int newIndex = (currentWeaponIndex + 1) % weaponsInformation.Length;
        Debug.Log($"[Weapons] Cycling from {currentWeaponIndex} ({weaponsInformation[currentWeaponIndex].weaponName}) to {newIndex} ({weaponsInformation[newIndex].weaponName})");
        currentWeaponIndex = newIndex;
        UpdateWeaponDisplay();
    }

    public void PreviousWeapon()
    {
        int newIndex = (currentWeaponIndex - 1 + weaponsInformation.Length) % weaponsInformation.Length;
        Debug.Log($"[Weapons] Cycling from {currentWeaponIndex} ({weaponsInformation[currentWeaponIndex].weaponName}) to {newIndex} ({weaponsInformation[newIndex].weaponName})");
        currentWeaponIndex = newIndex;
        UpdateWeaponDisplay();
    }

    public void UpdateWeaponDisplay()
    {
        if (weaponsInformation.Length == 0)
        {
            weaponNameText.text = "No weapons";
            return;
        }

        weaponNameText.text = weaponsInformation[currentWeaponIndex].weaponName;
        Debug.Log($"[Weapons] Now displaying: {weaponsInformation[currentWeaponIndex].weaponName}");
    }
    #endregion

    #region Enemy System
    public void SafeNextEnemy()
    {
        if (!enemyInputLock && enemiesInformation.Length > 1)
        {
            enemyInputLock = true;
            NextEnemy();
            enemyInputLock = false;
        }
    }

    public void SafePreviousEnemy()
    {
        if (!enemyInputLock && enemiesInformation.Length > 1)
        {
            enemyInputLock = true;
            PreviousEnemy();
            enemyInputLock = false;
        }
    }

    public void NextEnemy()
    {
        int newIndex = (currentEnemyIndex + 1) % enemiesInformation.Length;
        Debug.Log($"[Enemies] Cycling from {currentEnemyIndex} ({enemiesInformation[currentEnemyIndex].enemyName}) to {newIndex} ({enemiesInformation[newIndex].enemyName})");
        currentEnemyIndex = newIndex;
        UpdateEnemyDisplay();
    }

    public void PreviousEnemy()
    {
        int newIndex = (currentEnemyIndex - 1 + enemiesInformation.Length) % enemiesInformation.Length;
        Debug.Log($"[Enemies] Cycling from {currentEnemyIndex} ({enemiesInformation[currentEnemyIndex].enemyName}) to {newIndex} ({enemiesInformation[newIndex].enemyName})");
        currentEnemyIndex = newIndex;
        UpdateEnemyDisplay();
    }

    public void UpdateEnemyDisplay()
    {
        if (enemiesInformation.Length == 0)
        {
            enemyNameText.text = "No enemies";
            return;
        }

        enemyNameText.text = enemiesInformation[currentEnemyIndex].enemyName;
        Debug.Log($"[Enemies] Now displaying: {enemiesInformation[currentEnemyIndex].enemyName}");
    }
    #endregion
}