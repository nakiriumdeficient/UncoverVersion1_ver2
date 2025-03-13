using UnityEngine;

public class Drull : MonoBehaviour
{
    public int damage = 20; // Damage dealt by the Drull
    private Transform target; // Target to move towards (the player)

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * 10f * Time.deltaTime; // Adjust speed as needed
        }
        else
        {
            // If the target is gone, destroy the Drull
            Destroy(gameObject);
        }
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

            // Destroy the Drull on impact
            Destroy(gameObject);
        }
    }
}