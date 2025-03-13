using UnityEngine;

public class Captain : NPC
{
    public float speed = 3.5f;  // Captain's movement speed
    public float stopDistance = 2.5f;  // Stops moving when close enough to Grey
    private Animator animator;
    public float attackCooldown = 1.5f; // Time between attacks
    private float lastAttackTime = 0f;
    private bool isDead = false;

    protected override void Start()
    {
        npcName = "Captain";  // Set NPC name
        base.Start(); // ✅ Now it keeps the Inspector value

        // Try to find the child object dynamically
        Transform modelTransform = transform.Find("Captain_Model");

        if (modelTransform != null)
        {
            animator = modelTransform.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("[Captain] Child object 'Captain_Model' not found! Check the name in the Hierarchy.");
        }
    }

    protected override void Update()
    {
        if (isDead) return; // ✅ Prevents movement after death

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;
        }

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange && distance > stopDistance)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }

            // ✅ Enforce attack cooldown correctly
            if (distance <= stopDistance && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time; // ✅ Update cooldown timer
            }
        }

        bool shouldRun = isChasing;

        if (animator != null)
        {
            animator.SetBool("isRunning", shouldRun);
        }

        if (shouldRun)
        {
            FacePlayer();
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (controller == null || player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        direction.z = 0;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance) // Stop moving when close enough
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection.x > 0) // Player is to the right
        {
            transform.rotation = Quaternion.Euler(0, 90, 0); // Face right
        }
        else // Player is to the left
        {
            transform.rotation = Quaternion.Euler(0, -90, 0); // Face left
        }
    }

    public override void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // ✅ Use a trigger for one-time attack animation
        }
        Debug.Log("[Captain] Attacking Grey!");
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return; // ✅ Prevents taking damage after death

        currentHealth -= damage;
        Debug.Log("[Captain] Took " + damage + " damage! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // ✅ Call Die() function when health reaches 0
        }
    }
void Die()
{
    if (isDead) return;
    isDead = true;

    Debug.Log("[Captain] Has died!");

    if (animator != null)
    {
        animator.SetTrigger("Die"); // ✅ Use a trigger instead of a bool
    }

    float deathAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;
    Destroy(gameObject, deathAnimLength + 0.5f);
}
}
