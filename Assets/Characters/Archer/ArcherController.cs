using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArcherController : MonoBehaviour
{
    public GameObject arrowPrefab; // Reference to the arrow prefab
    public Transform arrowSpawnPoint; // Point where the arrow will be spawned
    public float attackRange = 10f; // Range within which the archer will attack
    public float attackCooldown = 2f; // Cooldown between attacks
    public int damage = 10; // Damage dealt by the arrow
    public int maxHealth = 1; // Maximum health of the archer

    private Transform player; // Reference to the player's transform
    private Animator animator; // Reference to the animator
    private bool isAttacking = false; // Flag to check if the archer is attacking
    private int currentHealth; // Current health of the archer
    private bool isDead = false; // Flag to check if the archer is dead

    public GameObject expOrb; // Assign XP orb prefab in the Inspector
    public int xpDropAmount = 30;
    public int numberOfXpDrops = 3; // Number of XP orbs to spawn

    public GameObject upgradeOrb;
    public int upgradeDropAmount = 30;
    public int numberOfUpDrops = 3;

    private Slider healthBar; // Assign in Inspector

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Get the Animator component
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component missing from Archer or its children!");
        }

        healthBar = GetComponentInChildren<Slider>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Start a coroutine to find the player
        StartCoroutine(FindPlayer());

        // Start the attack coroutine
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator FindPlayer()
    {
        while (player == null)
        {
            // Search for the player object with the "GreyPlayer" tag
            player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;
            if (player == null)
            {
                Debug.LogWarning("Player not found yet. Retrying...");
                yield return new WaitForSeconds(3.5f); // Wait before retrying
            }
            else
            {
                Debug.Log("Player found: " + player.name);
            }
        }
    }

    void Update()
    {
        healthBar.value = currentHealth;
        // Rotate the archer to face the player
        if (player != null && !isDead)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the archer upright
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking)
            {
                isAttacking = true;
                Debug.Log("Setting Attack trigger...");
                animator.SetTrigger("Attack"); // Trigger the attack animation

                // Wait for the animation to reach the frame where the arrow should be fired
                yield return new WaitForSeconds(0.5f); // Adjust this delay based on your animation

                FireArrow();

                // Wait for the cooldown before attacking again
                yield return new WaitForSeconds(attackCooldown);
                isAttacking = false;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void FireArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow prefab not set!");
            return;
        }

        if (arrowSpawnPoint == null)
        {
            Debug.LogError("Arrow spawn point not set!");
            return;
        }

        // Instantiate the arrow at the spawn point
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        // Get the Arrow script component and set its damage and target
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDamage(damage);
            arrowScript.SetTarget(player);
        }
        else
        {
            Debug.LogError("Arrow prefab is missing the Arrow script!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Ignore damage if already dead

        currentHealth -= damage;
        if (healthBar)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log("Archer took " + damage + " damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DropExperience();
        DropUpgrade();

        isDead = true;
        Debug.Log("Archer is dead!");

        // Trigger the death animation
        animator.SetTrigger("Die");

        // Disable the ArcherController script to stop further actions
        enabled = false;

        // Optionally, destroy the archer after a delay
        StartCoroutine(DestroyAfterDelay(3f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
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