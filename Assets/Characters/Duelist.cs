using UnityEngine;
using System.Collections;

public class Duelist : NPC
{
    public float moveSpeed = 2f;
    public float attackDistance = 1.5f; // Distance to start attacking
    public float gravity = -9.81f; // Gravity strength

    private float distanceToPlayer;
    private Vector3 velocity; // For gravity and vertical movement
    private Animator animator;

    protected override void Start()
    {
        npcName = "Duelist"; // Set the NPC name
        maxHealth = 50; // Set the max health

        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform; // Use "GreyPlayer" tag

        if (player == null)
        {
            Debug.LogError("[Duelist] Player not found! Make sure the player GameObject has the 'GreyPlayer' tag.");
        }

        // Get the Animator from the child object (assuming the model is a child)
        animator = transform.GetChild(0).GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on the child object. Make sure the model has an Animator component.");
        }
    }

    protected override void Update()
    {
        if (currentHealth <= 0) return; // Stop updating if the Duelist is dead

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
            animator.SetBool("isRunning", false); // Stop running animation
        }

        // Apply gravity to the CharacterController
        controller.Move(velocity * Time.deltaTime);
    }

    void MoveTowardsPlayer()
{
    Vector3 direction = (player.position - transform.position).normalized;
    direction.y = 0; // Ensure the character doesn't move vertically

    // Move the CharacterController
    controller.Move(direction * moveSpeed * Time.deltaTime);

    animator.SetBool("isRunning", true); // Play running animation

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
        animator.SetBool("isRunning", false); // Stop running animation

        // Trigger the attack animation (Attack is the trigger we added in the Animator)
        animator.SetTrigger("Attack");
    }

    public override void Attack()
    {
        base.Attack(); // Call the base NPC Attack method (optional)
        Debug.Log("[Duelist] Performing a special attack!");
    }

    protected override void Die()
    {
        base.Die(); // Call the base NPC Die method
        Debug.Log("[Duelist] Died! Playing death animation...");

        // Add death animation or other logic here
        if (animator != null)
        {
            animator.SetTrigger("Die"); // Play death animation
        }

        StartCoroutine(RemoveNPC());
    }

    private IEnumerator RemoveNPC()
    {
        yield return new WaitForSeconds(2.0f); // Wait for death animation
        Destroy(gameObject); // Remove NPC from scene
        Debug.Log("[Duelist] Removed from scene.");
    }
}