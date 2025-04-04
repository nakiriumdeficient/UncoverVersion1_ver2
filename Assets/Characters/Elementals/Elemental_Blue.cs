using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Diagnostics.Contracts;
using UnityEngine.UI;
using TMPro;

public class Elemental_Blue : MonoBehaviour
{
    public Slider hpSlider; // Assign in the Inspector
    public TextMeshProUGUI hpText; // Assign in the Inspector

    public float detectionRange = 5f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public int maxHealth = 50; // ✅ Set max health
    private int currentHealth;
    private float lastAttackTime = 0f;
    private Animator animator;
    private bool isAttacking = false;
    private GameObject player;
    public GameObject expOrb; // Assign XP orb prefab in the Inspector
    public int xpDropAmount = 30;
    public int numberOfXpDrops = 3; // Number of XP orbs to spawn

    public GameObject upgradeOrb;
    public int upgradeDropAmount = 30;
    public int numberOfUpDrops = 3;

    //  Boss System
    public bool isBoss = false; // Set in the Inspector if this is a boss
    public string bossID = ""; // Unique ID for the boss (e.g., "Boss1")
    private GameObject bossHP; // Reference to UI Prompt

    private Slider healthBar; // Assign in Inspector

    private void Start()
    {
        //  Prevent respawning if this is a boss that was already defeated
        if (isBoss && GameManager.Instance.defeatedBosses.Contains(bossID))
        {
            Destroy(gameObject);
            return; // Exit Start() to avoid running AI logic
        }
        if(isBoss)
        {
            bossHP = GameObject.FindObjectOfType<Canvas>().transform.Find("SageHPBar")?.gameObject;
            hpSlider = bossHP?.GetComponentInChildren<Slider>();
            hpText = bossHP?.GetComponentInChildren<TextMeshProUGUI>();

            if (bossHP != null)
            {
                bossHP.SetActive(false); // Start disabled
            }
        }

        if(!isBoss)
        {
            healthBar = GetComponentInChildren<Slider>();

            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth;
                healthBar.value = currentHealth;
            }
        }

        StartCoroutine(FindPlayer()); // Start looking for the player
    }

    private void Update()
    {
        updateHPBar();
        if (player == null || isAttacking) return;

        healthBar.value = currentHealth;

        if (player.transform == null) return; // Prevents null reference error

        float distance = Mathf.Abs(player.transform.position.x - transform.position.x); // Only check X distance

        if (distance <= detectionRange)
        {
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time; // ✅ Reset cooldown timer
            }
        }
        
        updateHPBar();
        
    }
    private IEnumerator FindPlayer()
    {
        while (player == null) // Keep checking until the player is found
        {
            player = GameObject.FindGameObjectWithTag("GreyPlayer"); // Ensure your player has this tag
            yield return new WaitForSeconds(0.2f); // Check every 0.2 seconds
        }

        Debug.Log("Player found: " + player.name);
        InitializeAI(); // Call a function to set up AI after finding the player
    }
    void InitializeAI()
    {
        animator = GetComponentInChildren<Animator>(); // Animator should be on the model child
        currentHealth = maxHealth; // ✅ Initialize health
        Debug.Log("[Blue_Elemental] Initialized with HP: " + maxHealth);
        Debug.Log("Enemy AI initialized!");
    }
    public void updateHPBar()
    {
        if (hpSlider == null || hpText == null) return; // Prevents null reference errors

        hpSlider.maxValue = 200;
        hpSlider.value = currentHealth;

        hpText.text = $"{currentHealth} / {maxHealth}";
    }
    void Attack()
    {
        isAttacking = true;
        Debug.Log("[Blue_Elemental] Attacking!");
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // ✅ Play attack animation
        }
        Invoke("ResetAttack", attackCooldown); // ✅ Reset attack state after cooldown
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("[Blue_Elemental] Got hit! Incoming damage: " + damage);
        currentHealth -= damage;
        Debug.Log("[Blue_Elemental] Took " + damage + " damage! HP: " + currentHealth);

        if (healthBar)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        updateHPBar();
        DropExperience();
        DropUpgrade();
        StartCoroutine(Waitforfive());
        Debug.Log("[Blue_Elemental] Defeated!");

        // If this is a boss, mark it as defeated in save data
        if (isBoss)
        {
            GameManager.Instance.defeatedBosses.Add(bossID);
            GameManager.Instance.SaveGame();
            bossHP.SetActive(false);
        }

        Destroy(gameObject); // Remove Blue_Elemental from the scene when dead
    }
    private IEnumerator Waitforfive()
    {
        Debug.Log("waiting for 2 sec");
        yield return new WaitForSeconds(2f); // Ensures scene objects exist
    }
    private void DropExperience()
    {
        for (int i = 0; i < numberOfXpDrops; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-0.1f, 0.1f));
            GameObject xp = Instantiate(expOrb, transform.position + randomOffset, Quaternion.identity);
            ExperiencePickup xpScript = xp.GetComponent<ExperiencePickup>();
            if (xpScript != null)
            {
                xpScript.expAmount = xpDropAmount / numberOfXpDrops; // Distribute XP evenly
            }
        }
    }
    private void DropUpgrade()
    {
        for (int i = 0; i < numberOfUpDrops; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0.2f, Random.Range(-0.1f, 0.1f));
            GameObject upgrade = Instantiate(upgradeOrb, transform.position + randomOffset, Quaternion.identity);
            UpgradePickup upgradeScript = upgrade.GetComponent<UpgradePickup>();
            if (upgradeScript != null)
            {
                upgradeScript.upgradeAmount = upgradeDropAmount / numberOfXpDrops;
            }
        }

    }
}
