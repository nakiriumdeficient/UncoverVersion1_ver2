using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName; // Name of the item (e.g., "Key2", "Key3")

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            // Notify the GameManager
            GameManager.Instance.CollectItem(itemName);

            // Notify the ObjectiveManager
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.CollectItem(itemName);
            }

            Destroy(gameObject); // Remove the item after collection
        }
    }
}