using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Captain : NPC
{
    public Slider hpSlider; // Assign in the Inspector
    public TextMeshProUGUI hpText; // Assign in the Inspector

    public float speed = 3.5f;  // Captain's movement speed
    public float stopDistance = 2.5f;  // Stops moving when close enough to Grey
    private Animator animator;
    public float attackCooldown = 1.5f; // Time between attacks
    private float lastAttackTime = 0f;
    private bool isDead = false;

    public GameObject expOrb; // Assign XP orb prefab in the Inspector
    public int xpDropAmount = 30;
    public int numberOfXpDrops = 3; // Number of XP orbs to spawn

    public GameObject upgradeOrb;
    public int upgradeDropAmount = 30;
    public int numberOfUpDrops = 3;

    //  Boss System
    public bool isBoss = false; // Set in the Inspector if this is a boss
    private GameObject bossHP; // Reference to UI Prompt
    public string bossID = "";

    private Slider healthBar; // Assign in Inspector
    protected override void Start()
    {
        if(GameManager.Instance.defeatedBosses.Contains(bossID))
        {
            Destroy(gameObject);
            return; // Exit Start() to avoid running AI logic
        }
        
        base.Start(); // Now it keeps the Inspector value

        if (isBoss)
        {
            bossHP = GameObject.FindObjectOfType<Canvas>().transform.Find("CaptainHPBar")?.gameObject;
            hpSlider = bossHP?.GetComponentInChildren<Slider>();
            hpText = bossHP?.GetComponentInChildren<TextMeshProUGUI>();

            if (bossHP != null)
            {
                bossHP.SetActive(false); // Start disabled
            }
        }

        if (!isBoss)
        {
            healthBar = GetComponentInChildren<Slider>();

            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth;
                healthBar.value = currentHealth;
            }
        }


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
        if (isDead) return; // Prevents movement after death
        updateHPBar();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;
        }

        healthBar.value = currentHealth;

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

        updateHPBar();

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
    public void updateHPBar()
    {
        hpSlider.maxValue = 200;
        hpSlider.value = currentHealth;

        hpText.text = $"{currentHealth} / {maxHealth}";
            
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

        if (healthBar)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die(); // ✅ Call Die() function when health reaches 0
        }
    }
    void Die()
    {
        bossHP = GameObject.FindObjectOfType<Canvas>().transform.Find("CaptainHPBar")?.gameObject;
        updateHPBar();

        updateHPBar();
        if (!GameManager.Instance.IsEnemyDefeated(enemyID))  // Check if the enemy is already defeated
        {
            GameManager.Instance.MarkEnemyAsDefeated(enemyID);  // Mark it as defeated
            DropExperience();  // Drop XP and upgrades only if not defeated before
            DropUpgrade();
        }
        // If this is a boss, mark it as defeated in save data
        if (isBoss)
        {
            GameManager.Instance.defeatedBosses.Add(bossID);
            GameManager.Instance.SaveGame();
            bossHP.SetActive(false);
        }

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
