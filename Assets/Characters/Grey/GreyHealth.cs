using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreyHealth : MonoBehaviour
{
    
    public HealthBar healthBar;
    private bool isDead = false;
    Animator animator;
    private PlayerSpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(GameManager.Instance.playermaxHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;


        GameManager.Instance.playercurHP -= damage;
        Debug.Log("[Grey] Took " + damage + " damage! HP: " + GameManager.Instance.playercurHP);
        healthBar.SetHealth(GameManager.Instance.playercurHP);

        if (GameManager.Instance.playercurHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        Debug.Log("[Grey] HP reached 0! Playing death animation...");
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die"); // Play death animation
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.0f); // Wait for death animation

        if (spawnManager != null)
        {
            spawnManager.RespawnGreyPlayer(); // Use PlayerSpawnManager for respawn
        }
        else
        {
            Debug.LogError("PlayerSpawnManager missing! Cannot respawn.");
        }

        GameManager.Instance.playercurHP = GameManager.Instance.playermaxHP; // Restore health
        isDead = false;

        Debug.Log("[Grey] Respawned at PlayerSpawnManager's default position with full HP: " + GameManager.Instance.playermaxHP);
    }
}
