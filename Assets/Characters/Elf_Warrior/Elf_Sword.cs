using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int damage = 10; // Damage dealt to Grey

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // ✅ Check if we hit Grey
        {
            GreyHealth grey = other.GetComponent<GreyHealth>();
            if (grey != null)
            {
                grey.TakeDamage(damage); // ✅ Apply damage to Grey
                Debug.Log("[Elf_Sword] Hit Grey for " + damage + " damage!");
            }
        }
    }
}
