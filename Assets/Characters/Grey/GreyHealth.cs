using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using static Unity.VisualScripting.Member;

public class GreyHealth : MonoBehaviour
{
    public event Action OnDeath;
    public HealthBar healthBar;
    private bool isDead = false;
    Animator animator;

    private GameObject deathScreen; // Assign in Inspector
    private TextMeshProUGUI deathMessage; // Reference to Death Screen text
    private string lastDamageSource = ""; // Track how the player died
    // Start is called before the first frame update

    public bool canBeDamaged = true;
    void Start()
    {
        healthBar.SetMaxHealth(GameManager.Instance.playermaxHP);
        deathScreen = GameObject.FindObjectOfType<Canvas>().transform.Find("DeathScreen")?.gameObject;

        if (deathScreen != null)
        {
            deathScreen.SetActive(false); // Ensure it's hidden at start
            deathMessage = deathScreen.transform.Find("DeathMessage")?.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("DeathScreen not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(GameManager.Instance.playercurHP);
    }
    public void TakeDamage(int damage, string source)
    {
        if (isDead) return;

        if (!canBeDamaged) return;

        GameManager.Instance.playercurHP -= damage;
        lastDamageSource = source; // Store last hit source

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
        if (deathScreen != null)
        {
            deathScreen.SetActive(true); // Show Death Screen

            if (deathMessage != null)
            {
                // Change text based on death source
                if (lastDamageSource == "trap")
                {
                    deathMessage.text = "You stepped on deadly traps...";
                }
                else if (lastDamageSource == "enemy")
                {
                    deathMessage.text = "A foe has bested you in battle...";
                }
                else
                {
                    deathMessage.text = "You have perished...";
                }
            }
        }

        OnDeath?.Invoke();
        Time.timeScale = 0; // Freeze game
    }
}
