using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup2 : MonoBehaviour
{
    public string weaponName;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // Ensure only player can pick up
        {
            GameManager.Instance.CollectWeapon(weaponName, damage);
            Debug.Log("Picked up: " + weaponName);

            Destroy(gameObject); // Remove from scene
        }
    }
}
