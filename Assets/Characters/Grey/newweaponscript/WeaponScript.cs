using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private Collider weaponCollider;
    public WeaponData currentWeapon;

    void Start()
    {
        weaponCollider = GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false; // Disable collider when idle
        }
        else
        {
            Debug.LogError("[Weapon] No Collider found on " + gameObject.name);
        }

        // Set damage based on weapon type
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Weapon hit: " + other.gameObject.name); // Log what it hit

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Eenemy Hit!");

            NPC npc = other.GetComponent<NPC>();

            ElfWarrior elfwarrior = other.GetComponent<ElfWarrior>();
            ArcherController archer = other.GetComponent<ArcherController>();

            if (currentWeapon == null)
            {
                Debug.LogError("Error: currentWeapon is NULL!");
                return;
            }




            if (npc != null)
            {
                npc.TakeDamage(currentWeapon.damage);
                return; // Stops execution here if it's an NPC
            }

            else if (elfwarrior != null)
            {
                elfwarrior.TakeDamage(currentWeapon.damage);
                return;
            }
            else if (archer != null)
            {
                Debug.Log("Dealing damage: " + currentWeapon.damage);
                archer.TakeDamage(currentWeapon.damage);
                Debug.Log("[Weapon] Dealt " + currentWeapon.damage + " damage to Archer: " + other.name);
                return;
            }
            else
            {
                Debug.LogError("Error: Enemy script not found on " + other.gameObject.name);
            }

        }
    }
}
