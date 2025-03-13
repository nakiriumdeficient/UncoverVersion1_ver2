using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int experience = 0;

    void Start()
    {
        LoadGame();
    }

    public void GainExp(int amount)
    {
        experience += amount;
        if (experience >= level * 100)  // Example: Level up when EXP reaches 100 * current level
        {
            experience = 0;
            level++;
            Debug.Log("Level Up! New Level: " + level);
        }
    }

    

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
            level = data.level;
            experience = data.experience;
            Debug.Log("Game Loaded!");
        }
    }
}
