using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Captain : NPC
{
    [Header("Boss UI")]
    public bool isBoss = false;
    public string bossID = "";
    private GameObject bossHP;
    private Slider hpSlider;
    private TextMeshProUGUI hpText;

    [Header("Combat Stats")]
    public float speed = 3.5f;
    public float stopDistance = 3.5f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;
    private bool isDead = false;

    [Header("Drops")]
    public GameObject expOrb;
    public int xpDropAmount = 30;
    public int numberOfXpDrops = 3;

    public GameObject upgradeOrb;
    public int upgradeDropAmount = 30;
    public int numberOfUpDrops = 3;

    private Animator animator;
    private bool isAttacking = false;
    [Header("Attack Behavior")]
    public float attackPush = 1.5f; // how far forward he slides during attack

    protected override void Start()
    {
        // Destroy if already defeated
        if (GameManager.Instance.defeatedBosses.Contains(bossID))
        {
            Destroy(gameObject);
            return;
        }

        base.Start(); // from NPC

        // Initialize Animator
        Transform modelTransform = transform.Find("Captain_Model");
        if (modelTransform)
            animator = modelTransform.GetComponent<Animator>();

        // Try to find boss HP bar once (on player’s Canvas)
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas != null)
        {
            Transform hpBar = mainCanvas.transform.Find("CaptainHPBar");
            if (hpBar != null)
            {
                bossHP = hpBar.gameObject;
                hpSlider = bossHP.GetComponentInChildren<Slider>();
                hpText = bossHP.GetComponentInChildren<TextMeshProUGUI>();
                bossHP.SetActive(false); // start hidden
            }
            else
            {
                Debug.LogWarning("[Captain] CaptainHPBar not found on canvas!");
            }
        }

        if (mainCanvas != null)
        {
            Debug.Log("[Captain] Found canvas: " + mainCanvas.name);
            Transform hpBar = mainCanvas.transform.Find("CaptainHPBar");
            if (hpBar != null)
                Debug.Log("[Captain] Found CaptainHPBar inside canvas!");
            else
                Debug.LogWarning("[Captain] CaptainHPBar not found inside " + mainCanvas.name);
        }
        else
        {
            Debug.LogError("[Captain] No Canvas found in the scene!");
        }
    }

    protected override void Update()
    {
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (isDead) return;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // ✅ Activate boss HP when player is close enough
        if (isBoss && bossHP != null && !bossHP.activeSelf && distance <= detectionRange)
            bossHP.SetActive(true);

        //  Update HP bar only if visible
        if (isBoss && bossHP != null && bossHP.activeSelf)
            UpdateHPBar();

        //  If attacking, don't chase or move
        if (isAttacking)
        {
            // Optional: Apply a small forward slide during the attack
            Vector3 push = transform.forward * attackPush * Time.deltaTime;
            controller.Move(push);
            return;
        }

        //  Chase logic
        isChasing = (distance <= detectionRange && distance > stopDistance);

        if (isChasing)
        {
            FacePlayer();
            ChasePlayer();
            animator?.SetBool("isRunning", true);
        }
        else
        {
            animator?.SetBool("isRunning", false);
        }

        // ✅ Attack logic
        if (distance <= stopDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void UpdateHPBar()
    {
        if (hpSlider == null || hpText == null) return;

        hpSlider.maxValue = maxHealth;
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
        if (isDead || isAttacking) return;

        isAttacking = true;
        animator?.SetTrigger("Attack");

        //  Start coroutine to re-enable chasing after animation finishes
        StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        // Optional: get actual attack animation duration
        float attackTime = 4.1f;
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack")) // Replace with your animation clip name if needed
                attackTime = stateInfo.length;
        }

        yield return new WaitForSeconds(attackTime);

        isAttacking = false;
    }


    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHPBar();

        if (currentHealth <= 0)
            Die();
    }
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator?.SetTrigger("Die");
        if (isBoss && bossHP != null)
            bossHP.SetActive(false);

        GameManager.Instance.defeatedBosses.Add(bossID);
        GameManager.Instance.SaveGame();

        DropExperience();
        DropUpgrade();

        Destroy(gameObject, 2f);
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
