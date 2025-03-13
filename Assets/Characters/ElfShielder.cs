using UnityEngine;
using System.Collections;

public class ElfShielder : NPC
{
    public float speed = 4.0f;
    public float stopDistance = 2.0f;
    public float attackRange = 1.5f;
    private Animator animator;
    private bool isDead = false;

    protected override void Start()
    {
        npcName = "Elf Shielder";
        maxHealth = 100;
        base.Start();

        animator = GetComponentInChildren<Animator>(); // Get Animator from model
    }

    protected override void Update()
    {
        if (isDead) return; // ✅ Prevent movement when dead, but allow attacking before death

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;
        }

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange && distance > stopDistance)
            {
                isChasing = true; // ✅ Keep chasing until reaching stop distance
            }
            else
            {
                isChasing = false;
            }

            if (distance <= attackRange && !isDead) // ✅ Allow attack if not dead
            {
                isChasing = false; // Stop moving when close enough
                Attack();
            }
        }

        if (isChasing)
        {
            FacePlayer();
            ChasePlayer();
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    void ChasePlayer()
    {
        if (controller == null || player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;  // Prevent Y-axis movement (keep on the ground plane)
        direction.z = 0;  // Prevent Z-axis movement (ensure 2D movement)

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)  // Stop moving when close enough
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;  // Ensure NPC stays on the ground plane (no Y-axis rotation)

        if (lookDirection.x > 0)
        {
            // Facing right
            transform.rotation = Quaternion.Euler(0, 180, 0); 
        }
        else
        {
            // Facing left
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
    }

    public override void Attack()
    {
        if (animator != null)
        {
            FacePlayer(); // Ensure the NPC faces the player before attacking
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
            Debug.Log("[Elf Shielder] Attacking Grey!");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return; // ✅ Prevent taking damage when already dead

        currentHealth -= damage;
        Debug.Log("[Elf Shielder] Took " + damage + " damage! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        Debug.Log("[Elf Shielder] Died! Playing death animation...");
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die"); // ✅ Play death animation
        }

        StartCoroutine(RemoveNPC());
    }

    private IEnumerator RemoveNPC()
    {
        yield return new WaitForSeconds(2.0f); // ✅ Wait for death animation

        Destroy(gameObject); // ✅ Remove NPC from scene
        Debug.Log("[Elf Shielder] Removed from scene.");
    }
}
