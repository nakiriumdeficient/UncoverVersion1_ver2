using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "Enemy"; // Name of the NPC
    public int maxHealth = 50;
    protected int currentHealth;

    public float detectionRange = 5.0f; // Detection distance
    protected Transform player;
    protected bool isChasing = false;
    protected CharacterController controller;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("GreyPlayer")?.transform;

        Debug.Log("[NPC] " + npcName + " spawned with " + maxHealth + " HP.");
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("[NPC] " + npcName + " took " + damage + " damage! HP left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("[NPC] " + npcName + " has been defeated!");
        Destroy(gameObject);
    }

    public virtual void Attack()
    {
        Debug.Log("[NPC] " + npcName + " attacks! (Override this in child classes)");
    }
}
