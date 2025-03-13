using UnityEngine;

public class Elf_Shield : MonoBehaviour
{
    public int damage = 20; // ✅ High damage due to heavy shield

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // ✅ Check if hitting Grey
        {
            GreyHealth grey = other.GetComponent<GreyHealth>();
            if (grey != null)
            {
                grey.TakeDamage(damage); // ✅ Apply damage to Grey
                Debug.Log("[Elf Shield] Slammed into Grey! Dealt " + damage + " damage.");
            }
        }
    }
}
