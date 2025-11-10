using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AelfricController : MonoBehaviour
{
    public Slider hpSlider; // Assign in the Inspector
    public TextMeshProUGUI hpText; // Assign in the Inspector

    public GameObject drullPrefab;
    public Transform drullSpawnPoint;
    public float chaseRange = 10f;
    public float attackRange = 5f;
    public float attackCooldown = 3f;
    public int damage = 20;
    public int maxHealth = 100;
    public float moveSpeed = 3f;

    public GameObject expOrb; // Assign XP orb prefab in the Inspector
    public int xpDropAmount = 30;
    public int numberOfXpDrops = 3; // Number of XP orbs to spawn

    public GameObject upgradeOrb;
    public int upgradeDropAmount = 30;
    public int numberOfUpDrops = 3;

    public FadeTransition fadeTransition; // Assign this in the Inspector

    private Transform player;
    private Animator animator;
    private Rigidbody rb;
    private bool isAttacking = false;
    private bool isChasing = false;
    private int currentHealth;
    private bool isDead = false;


    private GameObject bossHP; // Reference to UI Prompt
    public string bossID = "";
    void Start()
    {
        if (GameManager.Instance.defeatedBosses.Contains(bossID))
        {
            Destroy(gameObject);
            return;
        }
        bossHP = GameObject.FindObjectOfType<Canvas>().transform.Find("AelfricHPBar")?.gameObject;
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from Aelfric or its children!");
        }

        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

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
                yield return new WaitForSeconds(0.5f); // Wait before retrying
            }
            else
            {
                Debug.Log("Player found: " + player.name);
            }
        }
    }

    void Update()
    {
        if (isDead || player == null) return;
        updateHPBar();

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
    public void updateHPBar()
    {
        hpSlider.maxValue = 100;
        hpSlider.value = currentHealth;

        hpText.text = $"{currentHealth} / {maxHealth}";

    }
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Ignore vertical difference
        if (direction.magnitude == 0) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            if (player == null)
            {
                yield return null; // Wait until the player is found
                continue;
            }

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
        updateHPBar();
        DropExperience();
        DropUpgrade();
        bossHP.SetActive(false);

        GameManager.Instance.defeatedBosses.Add(bossID);

        isDead = true;
        Debug.Log("Aelfric is dead!");
        animator.SetTrigger("Die");
        enabled = false;

        // Start the fade transition and load Level 37
        StartCoroutine(FadeAndLoadLevel());
    }

    private IEnumerator FadeAndLoadLevel()
    {
        // Fade to black
        if (fadeTransition != null)
        {
            yield return fadeTransition.FadeToBlack();
        }

        // Load Level 37
        SceneManager.LoadScene("Level37");

        // Fade from black (optional, if you want to fade in the next scene)
        if (fadeTransition != null)
        {
            yield return fadeTransition.FadeFromBlack();
        }
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
                upgradeScript.upgradeAmount = upgradeDropAmount / numberOfUpDrops;
            }
        }
    }
}