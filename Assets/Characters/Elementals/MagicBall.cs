using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public int damage = 10; // Damage dealt to Grey
    public string playerTag = "GreyPlayer"; // Ensure it targets Grey

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            GreyHealth grey = other.GetComponent<GreyHealth>();
            if (grey != null)
            {
                grey.TakeDamage(damage);
                Debug.Log("[MagicBall] Hit Grey for " + damage + " damage!");
            }
        }
    }
}
