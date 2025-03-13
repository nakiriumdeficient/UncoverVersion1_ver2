using UnityEngine;

public class Elemental_Blue : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public int maxHealth = 50; // ✅ Set max health
    private int currentHealth;
    private float lastAttackTime = 0f;
    private Transform player;
    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;
        animator = GetComponentInChildren<Animator>(); // Animator should be on the model child
        currentHealth = maxHealth; // ✅ Initialize health
        Debug.Log("[Blue_Elemental] Initialized with HP: " + maxHealth);
    }

    private void Update()
    {
        if (player == null || isAttacking) return;
        float distance = Mathf.Abs(player.position.x - transform.position.x); // Only check X distance

        if (distance <= detectionRange)
        {
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time; // ✅ Reset cooldown timer
            }
        }
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
        Debug.Log("[Blue_Elemental] Defeated!");
        Destroy(gameObject); // ✅ Remove Blue_Elemental from the scene when dead
    }
}
