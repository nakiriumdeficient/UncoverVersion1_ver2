using System.Collections;
using UnityEngine;

public class AelfricController : MonoBehaviour
{
    public GameObject drullPrefab;
    public Transform drullSpawnPoint;
    public float chaseRange = 10f;
    public float attackRange = 5f;
    public float attackCooldown = 3f;
    public int damage = 20;
    public int maxHealth = 100;
    public float moveSpeed = 3f;

    private Transform player;
    private Animator animator;
    private Rigidbody rb;
    private bool isAttacking = false;
    private bool isChasing = false;
    private int currentHealth;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player is tagged as 'GreyPlayer'.");
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from Aelfric or its children!");
        }

        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            if (distanceToPlayer > attackRange)
            {
                isChasing = true;
                animator.SetBool("IsRunning", true);
                MoveTowardsPlayer();
            }
            else
            {
                isChasing = false;
                rb.velocity = Vector3.zero;
                animator.SetBool("IsRunning", false);
            }

            RotateTowardsPlayer();
        }
        else
        {
            isChasing = false;
            animator.SetBool("IsRunning", false);
            rb.velocity = Vector3.zero;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                ThrowDrull();
                yield return new WaitForSeconds(attackCooldown);
                isAttacking = false;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void ThrowDrull()
    {
        if (drullPrefab != null && drullSpawnPoint != null)
        {
            GameObject drull = Instantiate(drullPrefab, drullSpawnPoint.position, drullSpawnPoint.rotation);
            Drull drullScript = drull.GetComponent<Drull>();
            if (drullScript != null)
            {
                drullScript.SetDamage(damage);
                drullScript.SetTarget(player);
            }
            else
            {
                Debug.LogError("Drull prefab is missing the Drull script!");
            }
        }
        else
        {
            Debug.LogError("Drull prefab or spawn point not set!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Aelfric took " + damage + " damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Aelfric is dead!");
        animator.SetTrigger("Die");
        enabled = false;
        StartCoroutine(DestroyAfterDelay(3f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
