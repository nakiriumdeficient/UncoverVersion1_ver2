using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public string bossID = ""; // Unique ID for this boss

    // Start is called before the first frame update
    void Start()
    {
        // Check if the boss was already defeated
        if (GameManager.Instance.defeatedBosses.Contains(bossID))
        {
            Destroy(gameObject); // Prevent respawning
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBossDefeated()
    {
        GameManager.Instance.defeatedBosses.Add(bossID);
        GameManager.Instance.SaveGame();
    }
}
