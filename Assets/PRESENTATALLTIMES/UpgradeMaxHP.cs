using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required for Button interactions

public class UpgradeMaxHP : MonoBehaviour
{
    private TextMeshProUGUI maxHPmax;
    private TextMeshProUGUI maxhpText;
    private TextMeshProUGUI upgradeCostText;
    private TextMeshProUGUI upgradeIncreaseText;
    private GameObject upgradeUI;
    private Button upgradeButton;

    private int maxUpgrades = 10; // Max times HP can be upgraded
    private int baseCost = 20; // Starting cost in upgrade orbs

    private int MaxHPUpgradesAllowed => GameManager.Instance.playerLevel / 2 + 5; // Scales with level
    void Start()
    {
        upgradeUI = GameObject.FindObjectOfType<Canvas>().transform.Find("UpgradeUI")?.gameObject;
        if (upgradeUI != null)
        {
            maxhpText = upgradeUI.transform.Find("MaxHP")?.GetComponent<TextMeshProUGUI>();
            upgradeCostText = upgradeUI.transform.Find("CostHP")?.GetComponent<TextMeshProUGUI>(); // Assign upgrade cost UI
            maxHPmax = upgradeUI.transform.Find("MaxHPUpgrade")?.GetComponent<TextMeshProUGUI>(); // Assign upgrade cost UI
            upgradeIncreaseText = upgradeUI.transform.Find("IncreaseHP")?.GetComponent<TextMeshProUGUI>(); // Assign upgrade cost UI
            upgradeButton = upgradeUI.transform.Find("UpgradeHP")?.GetComponent<Button>(); // Find the upgrade button
        }

        UpdateUI(); // Ensure UI is updated at the start
    }

    void Update()
    {
        UpdateUI();
    }

    public void PermaHPUpgrade()
    {
        int currentUpgrades = GameManager.Instance.hpUpgradeCount;
        int maxUpgrades = MaxHPUpgradesAllowed; // Use dynamic max upgrade count
        int currentCost = baseCost + (currentUpgrades * 5); // Cost increases by 5 per upgrade

        if (GameManager.Instance.upgradeOrb < currentCost)
        {
            Debug.Log("Not enough upgrade orbs!");
            UpdateUI();
            return;
        }

        if (currentUpgrades >= maxUpgrades)
        {
            Debug.Log("Max HP upgrades reached!");
            UpdateUI();
            return;
        }

        // Deduct upgrade orbs
        GameManager.Instance.upgradeOrb -= currentCost;

        // Increase Max HP and fully heal
        GameManager.Instance.playermaxHP += 10;
        GameManager.Instance.playercurHP = GameManager.Instance.playermaxHP;

        // Increase upgrade count in GameManager
        GameManager.Instance.hpUpgradeCount++;

        UpdateUI(); // Update UI with new values
    }

    private void UpdateUI()
    {
        int currentUpgrades = GameManager.Instance.hpUpgradeCount;
        int maxUpgrades = MaxHPUpgradesAllowed; // Get current max allowed upgrades
        int nextCost = baseCost + (currentUpgrades * 5);
        FindObjectOfType<SavePoint>().orbText.text = "Upgrade Orbs: " + GameManager.Instance.upgradeOrb;

        if (maxhpText != null)
        {
            maxhpText.text = $"Max HP: {GameManager.Instance.playermaxHP}";
        }

        if (upgradeCostText != null)
        {
            if (currentUpgrades >= maxUpgrades)
            {
                upgradeCostText.gameObject.SetActive(false); // Hide cost when maxed
                upgradeIncreaseText.gameObject.SetActive(false);
            }
            else
            {
                upgradeCostText.gameObject.SetActive(true);
                upgradeCostText.text = $"Cost: {nextCost} Orbs";
            }
        }

        if (maxHPmax != null)
        {
            if (currentUpgrades >= maxUpgrades)
            {
                maxHPmax.text = "Max HP upgrade achieved!\nIncrease levels for more upgrades!";
                maxHPmax.gameObject.SetActive(true);
            }
            else
            {
                maxHPmax.gameObject.SetActive(false);
            }
        }

        if (upgradeButton != null)
        {
            upgradeButton.interactable = (currentUpgrades < maxUpgrades);
        }

    }
}
