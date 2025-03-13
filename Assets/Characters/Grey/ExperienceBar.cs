using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    public Slider xpSlider; // Assign in the Inspector
    public TextMeshProUGUI xpText; // Assign in the Inspector
    public PlayerExperience playerExp; // Reference to the player XP script
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI orbs;

    private void Start()
    {
        playerExp = FindObjectOfType<PlayerExperience>(); // Find PlayerExperience script
        UpdateXPBar();
    }

    private void Update()
    {
        UpdateXPBar();
    }

    public void UpdateXPBar()
    {
        xpSlider.maxValue = GameManager.Instance.maxXP;
        xpSlider.value = GameManager.Instance.playerXP;
        xpText.text = $"{GameManager.Instance.playerXP} / {GameManager.Instance.maxXP}";
        levelText.text = $"Level: {GameManager.Instance.playerLevel}";
        orbs.text = $"Orbs: {GameManager.Instance.upgradeOrb}";

    }
}

