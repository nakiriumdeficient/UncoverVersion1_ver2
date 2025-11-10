using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementalBoss : MonoBehaviour
{
    [Header("Boss Info")]
    public string bossID = "Boss1";
    public int maxHealth = 300;
    private int currentHealth;
    private bool isDefeated = false;

    [Header("Combat")]
    public float detectionRange = 8f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    [Header("References")]
    private GameObject player;
    private Animator animator;

    [Header("UI")]
    private GameObject bossHPUI;
    private Slider hpSlider;
    private TextMeshProUGUI hpText;

    [Header("Drops")]
    public GameObject expOrb;
    public int xpDropAmount = 100;
    public int numberOfXpDrops = 5;

    public GameObject upgradeOrb;
    public int upgradeDropAmount = 50;
    public int numberOfUpDrops = 3;

    private void Start()
    {
        // Skip if already defeated in save data
        if (GameManager.Instance.defeatedBosses.Contains(bossID))
        {
            Destroy(gameObject);
            return;
        }

        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(FindPlayer());

        // Locate boss UI in scene
        bossHPUI = GameObject.FindObjectOfType<Canvas>()?.transform.Find("SageHPBar")?.gameObject;
        if (bossHPUI != null)
        {
            hpSlider = bossHPUI.GetComponentInChildren<Slider>();
            hpText = bossHPUI.GetComponentInChildren<TextMeshProUGUI>();
            bossHPUI.SetActive(false); // hide until battle starts
        }
    }

    private void Update()
    {
        if (player == null || isDefeated) return;

        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);

        if (distance <= detectionRange)
        {
            // Show HP bar when player approaches
            if (bossHPUI != null && !bossHPUI.activeSelf)
                bossHPUI.SetActive(true);

            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }

            FacePlayer();
        }

        UpdateHPBar();
    }

    private void FacePlayer()
    {
        if (player == null) return;
        Vector3 scale = transform.localScale;
        scale.x = player.transform.position.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private IEnumerator FindPlayer()
    {
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("GreyPlayer");
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator?.SetTrigger("Attack");
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDefeated) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHPBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        if (hpSlider == null || hpText == null) return;

        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;
        hpText.text = $"{currentHealth} / {maxHealth}";
    }

    private void Die()
    {
        isDefeated = true;

        animator?.SetTrigger("Die");
        GameManager.Instance.defeatedBosses.Add(bossID);
        GameManager.Instance.SaveGame();

        DropExperience();
        DropUpgrade();

        if (bossHPUI != null)
            bossHPUI.SetActive(false);

        Destroy(gameObject, 2f);
    }

    private void DropExperience()
    {
        if (expOrb == null) return;

        for (int i = 0; i < numberOfXpDrops; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0.5f, 0);
            GameObject xp = Instantiate(expOrb, transform.position + randomOffset, Quaternion.identity);
            if (xp.TryGetComponent(out ExperiencePickup xpScript))
                xpScript.expAmount = xpDropAmount / numberOfXpDrops;
        }
    }

    private void DropUpgrade()
    {
        if (upgradeOrb == null) return;

        for (int i = 0; i < numberOfUpDrops; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0.3f, 0);
            GameObject orb = Instantiate(upgradeOrb, transform.position + randomOffset, Quaternion.identity);
            if (orb.TryGetComponent(out UpgradePickup up))
                up.upgradeAmount = upgradeDropAmount / numberOfUpDrops;
        }
    }
}