using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    public Slider xpSlider; // Assign in the Inspector
    public TextMeshProUGUI xpText; // Assign in the Inspector
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI orbs;

    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    private void Start()
    {
  
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

        hpSlider.maxValue = (GameManager.Instance.playermaxHP);
        hpSlider.value = (GameManager.Instance.playercurHP);
        hpText.text = $"{GameManager.Instance.playercurHP} / {GameManager.Instance.playermaxHP}";

    }
}

