using UnityEngine;
using System.Collections;

public class Duelist : NPC
{
    public float moveSpeed = 2f;
    public float attackDistance = 1.5f; // Distance to start attacking
    public float gravity = -9.81f; // Gravity strength

    public GameObject expOrb; // Assign XP orb prefab in the Inspector
    public int xpDropAmount = 30;
    public int numberOfXpDrops = 3; // Number of XP orbs to spawn

    public GameObject upgradeOrb; // Assign upgrade orb prefab in the Inspector
    public int upgradeDropAmount = 30;
    public int numberOfUpDrops = 3; // Number of upgrade orbs to spawn

    private float distanceToPlayer;
    private Vector3 velocity; // For gravity and vertical movement
    private Animator animator;
    private bool isDead = false;

    protected override void Start()
    {
        npcName = "Duelist"; // Set the NPC name
        maxHealth = 50; // Set the max health

        base.Start(); // Call the base NPC Start method
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();

        // Get the Animator from the child object (assuming the model is a child)
        animator = transform.GetChild(0)?.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on the child object. Make sure the model has an Animator component.");
        }

        // Start a coroutine to find the player
        StartCoroutine(FindPlayer());
    }

    private IEnumerator FindPlayer()
    {
        while (player == null)
        {
            // Search for the player object with the "GreyPlayer" tag
            player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;
            if (player == null)
            {
                Debug.LogWarning("[Duelist] Player not found yet. Retrying...");
                yield return new WaitForSeconds(0.5f); // Wait before retrying
            }
            else
            {
                Debug.Log("[Duelist] Player found: " + player.name);
            }
        }
    }

    protected override void Update()
    {
        if (isDead || currentHealth <= 0) return; // Stop updating if the Duelist is dead

        if (player == null) return; // Exit if player is not found yet

        base.Update(); // Call the base NPC Update method to handle detection range

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Apply gravity
        if (controller.isGrounded)
        {
            velocity.y = 0f; // Reset vertical velocity when grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity
        }

        // Check if the player is within detection range
        if (isChasing)
        {
            // Apply movement or attack based on distance
            if (distanceToPlayer > attackDistance)
            {
                MoveTowardsPlayer();
            }
            else
            {
                PerformAttack();
            }
        }
        else
        {
            // Player is outside detection range, stop moving
            if (animator != null)
            {
                animator.SetBool("isRunning", false); // Stop running animation
            }
        }

        // Apply gravity to the CharacterController
        controller.Move(velocity * Time.deltaTime);
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Ensure the character doesn't move vertically

        // Move the CharacterController
        controller.Move(direction * moveSpeed * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("isRunning", true); // Play running animation
        }

        // Flip the model without resetting scale
        float originalScaleX = Mathf.Abs(transform.localScale.x); // Keep the original scale
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(originalScaleX, transform.localScale.y, transform.localScale.z); // Face right
        }
        else
        {
            transform.localScale = new Vector3(-originalScaleX, transform.localScale.y, transform.localScale.z); // Face left
        }
    }

    void PerformAttack()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", false); // Stop running animation
            animator.SetTrigger("Attack"); // Trigger attack animation
        }
    }

    public override void Attack()
    {
        base.Attack(); // Call the base NPC Attack method (optional)
        Debug.Log("[Duelist] Performing a special attack!");
    }

    protected override void Die()
    {
        if (isDead) return;

        isDead = true;

        base.Die(); // Call the base NPC Die method
        Debug.Log("[Duelist] Died! Playing death animation...");

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        DropExperience();
        DropUpgrade();

        StartCoroutine(RemoveNPC());
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
                upgradeScript.upgradeAmount = upgradeDropAmount / numberOfUpDrops;
            }
        }
    }

    private IEnumerator RemoveNPC()
    {
        yield return new WaitForSeconds(2.0f); // Wait for death animation
        Destroy(gameObject); // Remove NPC from scene
        Debug.Log("[Duelist] Removed from scene.");
    }
}