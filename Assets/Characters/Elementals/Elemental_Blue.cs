using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Diagnostics.Contracts;

public class Elemental_Blue : MonoBehaviour
{
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

    private void Start()
    {
        StartCoroutine(FindPlayer()); // Start looking for the player
    }

    private void Update()
    {
        if (player == null || isAttacking) return;

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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        DropExperience();
        DropUpgrade();

        Debug.Log("[Blue_Elemental] Defeated!");
        Destroy(gameObject); // ✅ Remove Blue_Elemental from the scene when dead
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
