using System.Collections;
using UnityEngine;

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

    void Start()
    {
       
        currentHealth = maxHealth;
        // Find the player by tag
    
        player = GameObject.FindGameObjectWithTag("GreyPlayer").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player is tagged as 'GreyPlayer'.");
        }

        // Get the Animator component
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component missing from Archer or its children!");
        }

        // Initialize health
        currentHealth = maxHealth;

        // Start the attack coroutine
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
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
        if (Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking)
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
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
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
        else
        {
            Debug.LogError("Arrow prefab or spawn point not set!");
        }
    }

    public void TakeDamage(int damage)
{
    if (isDead) return; // Ignore damage if already dead

    currentHealth -= damage;
    Debug.Log("Archer took " + damage + " damage! Current health: " + currentHealth);

    if (currentHealth <= 0)
    {
        Die();
    }
}

private void Die()
{
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
}