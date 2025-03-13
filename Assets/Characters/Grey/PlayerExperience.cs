using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public void AddExperience(int amount)
    {
        int currentXP = GameManager.Instance.playerXP += amount;

        CheckLevelUp();

        ExperienceBar xpBar = FindObjectOfType<ExperienceBar>();
        if (xpBar != null)
        {
            xpBar.UpdateXPBar();
        }
    }

    private void CheckLevelUp()
    {
        if (GameManager.Instance.playerXP >= GameManager.Instance.maxXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        GameManager.Instance.playerXP -= GameManager.Instance.maxXP; // Carry over extra XP
        GameManager.Instance.maxXP = Mathf.RoundToInt(GameManager.Instance.maxXP * 1.2f); // Increase XP requirement
        Debug.Log($"Leveled up to {GameManager.Instance.playerLevel}! Next level at {GameManager.Instance.maxXP} XP.");
    }
}
