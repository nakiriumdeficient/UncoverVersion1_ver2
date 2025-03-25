using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    public int damage = 100;
    private void OnTriggerEnter(Collider other)
    {
        // Check if Grey touched the spike
        GreyHealth grey = other.GetComponent<GreyHealth>();
        if (grey != null)
        {
            Debug.Log("Spike touched!");
            grey.TakeDamage(damage, "trap"); //  Apply damage to Grey
        }
    }
}
