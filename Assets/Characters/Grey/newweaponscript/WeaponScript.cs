using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private Collider weaponCollider;
    public WeaponData currentWeapon;

    // Damage multipliers for specific weapon-NPC combinations
    public float falxVsArcherMultiplier = 2.0f; // Example: Falx deals 2x damage to Archer
    public float shatterrackVsCaptainMultiplier = 1.3f; // Shatterrack deals 30% more damage to Captain

    // Crate destruction effect
    public GameObject crateDestroyEffect; // Assign the particle effect prefab in the Inspector

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
    }

    // Enable the weapon collider (call this when attacking)
    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            Debug.Log("Weapon collider enabled.");
        }
    }

    // Disable the weapon collider (call this after attacking)
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            Debug.Log("Weapon collider disabled.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Weapon hit: " + other.gameObject.name); // Log what it hit

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit!");

            NPC npc = other.GetComponent<NPC>();
            ElfWarrior elfwarrior = other.GetComponent<ElfWarrior>();
            ArcherController archer = other.GetComponent<ArcherController>();
            AelfricController aelfric = other.GetComponent<AelfricController>();
            Elemental_Blue elemental = other.GetComponent<Elemental_Blue>();
            ElfShielder shielder = other.GetComponent<ElfShielder>();
            Duelist duelist = other.GetComponent<Duelist>();
            Captain captain = other.GetComponent<Captain>();

            if (currentWeapon == null)
            {
                Debug.LogError("Error: currentWeapon is NULL!");
                return;
            }

            // Check if the weapon is Falx and the enemy is BlueElementalist
            if (currentWeapon.weaponName == "Falx" && elemental != null)
            {
                Debug.Log("Blue Elementalist is immune to Falx!");
                return; // Exit without dealing damage
            }

            // Calculate damage
            int damage = currentWeapon.damage;

            // Apply damage multiplier for Falx vs Archer
            if (currentWeapon.weaponName == "Falx" && archer != null)
            {
                damage = Mathf.RoundToInt(damage * falxVsArcherMultiplier);
                Debug.Log("Falx deals bonus damage to Archer! Damage: " + damage);
            }

            // Apply damage multiplier for shatterack vs Captain
            if (currentWeapon.weaponName == "Shatterack" && captain != null)
            {
                damage = Mathf.RoundToInt(damage * shatterrackVsCaptainMultiplier);
                Debug.Log("Shatterack deals bonus damage to Captain! Damage: " + damage);
            }

            // Deal damage to the enemy
            if (npc != null)
            {
                npc.TakeDamage(damage);
                return; // Stops execution here if it's an NPC
            }
            else if (elfwarrior != null)
            {
                elfwarrior.TakeDamage(damage);
                return;
            }
            else if (archer != null)
            {
                Debug.Log("Dealing damage: " + damage);
                archer.TakeDamage(damage);
                Debug.Log("[Weapon] Dealt " + damage + " damage to Archer: " + other.name);
                return;
            }
            else if (aelfric != null)
            {
                aelfric.TakeDamage(damage);
                return;
            }
            else if (elemental != null)
            {
                elemental.TakeDamage(damage);
                return;
            }
            else if (shielder != null)
            {
                shielder.TakeDamage(damage);
                return;
            }
            else if (duelist != null)
            {
                duelist.TakeDamage(damage);
                return;
            }
            else if (captain != null)
            {
                captain.TakeDamage(damage);
                return;
            }
            else
            {
                Debug.LogError("Error: Enemy script not found on " + other.gameObject.name);
            }
        }
        else if (other.CompareTag("Crate"))
        {
            Debug.Log("Crate Hit!");

            // Play the destroy sound
            AudioSource crateAudio = other.GetComponent<AudioSource>();
            if (crateAudio != null && crateAudio.clip != null)
            {
                crateAudio.Play();
                Debug.Log("Playing destroy sound: " + crateAudio.clip.name);
            }
            else
            {
                Debug.LogWarning("No AudioSource or AudioClip found on the crate.");
            }

            // Spawn the particle effect
            if (crateDestroyEffect != null)
            {
                Instantiate(crateDestroyEffect, other.transform.position, other.transform.rotation);
                Debug.Log("Spawned crate destroy effect.");
            }

            // Destroy the crate after a short delay (to allow the sound to play)
            Destroy(other.gameObject, 0.5f); // Adjust the delay as needed
        }
    }
}