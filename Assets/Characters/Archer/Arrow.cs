using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage; // Damage dealt by the arrow
    private Transform target; // Target to move towards
    public float speed = 10f; // Speed of the arrow

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Rotate the arrow to face the target
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            // If the target is null (e.g., player is dead), destroy the arrow
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            // Deal damage to the player
            GreyHealth player = other.GetComponent<GreyHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Destroy the arrow on hit
            Destroy(gameObject);
        }
    }
}