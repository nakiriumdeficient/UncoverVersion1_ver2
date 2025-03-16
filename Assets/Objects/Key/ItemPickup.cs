using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public string itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            GameManager.Instance.CollectItem(itemName);
            Destroy(gameObject); // Remove the item after collection
        }
    }
}
