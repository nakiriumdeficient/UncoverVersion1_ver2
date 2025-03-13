using UnityEngine;
using System.Collections;

public class ElfWarrior : NPC
{
    public float speed = 4.0f;
    public float stopDistance = 2.0f;
    public float attackRange = 1.5f;
    private Animator animator;
    private bool isDead = false;

    public GameObject expOrb; // Assign XP orb prefab in the Inspector
    public int xpDropAmount = 30;
    public int numberOfDrops = 3; // Number of XP orbs to spawn

    protected override void Start()
    {
        npcName = "Elf Warrior";
        maxHealth = 20;
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
        direction.y = 0;
        direction.z = 0;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance) // ✅ Stop moving when close enough
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void Attack()
    {
        if (animator != null)
        {
            FacePlayer();
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return; // ✅ Prevent taking damage when already dead

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die"); // ✅ Play death animation
        }

        DropExperience();

        StartCoroutine(RemoveNPC());
    }

    private void DropExperience()
    {
        for (int i = 0; i < numberOfDrops; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-0.1f, 0.1f));
            GameObject xp = Instantiate(expOrb, transform.position + randomOffset, Quaternion.identity);
            ExperiencePickup xpScript = xp.GetComponent<ExperiencePickup>();
            if (xpScript != null)
            {
                xpScript.expAmount = xpDropAmount / numberOfDrops; // Distribute XP evenly
            }
        }
    }

    private IEnumerator RemoveNPC()
    {
        yield return new WaitForSeconds(2.0f); // ✅ Wait for death animation

        Destroy(gameObject); // ✅ Remove NPC from scene
    }
}
