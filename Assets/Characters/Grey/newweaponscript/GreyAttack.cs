using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GreyAttack : MonoBehaviour
{
    public Transform weaponHolder;
    private WeaponData currentWeapon;
    private GameObject equippedWeaponObject;
    private Collider weaponCollider;
    private CharacterController controller;
    private Animator animator;
    public AudioSource attackAudioSource; // Reference to the AudioSource for attack sounds
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;
    private void Start()
    {
        EquipWeapon("Default Sword"); // Ensure a starting weapon exists
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Press 1 to equip the first collected weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeaponFromIndex(0);
        }
        // Press 2 to equip the second collected weapon
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeaponFromIndex(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeaponFromIndex(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipWeaponFromIndex(3);
        }

        if (controller.isGrounded)
        {
            if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
            
        }

    }

    public void EquipWeapon(string weaponName)
    {
        WeaponData weapon = GameManager.Instance.GetWeapon(weaponName);

        if (weapon != null)
        {
            currentWeapon = weapon;
            ActivateWeaponObject(weaponName); // Enable the weapon in the scene
            Debug.Log("Equipped: " + weapon.weaponName);
        }
        else
        {
            Debug.Log("Weapon not collected yet!");
        }
    }


    private void EquipWeaponFromIndex(int index)
    {
        if (GameManager.Instance.collectedWeapons.Count > index)
        {
            EquipWeapon(GameManager.Instance.collectedWeapons[index].weaponName);
        }
        else
        {
            Debug.Log("No weapon available in this slot!");
        }
    }
    private void ActivateWeaponObject(string weaponName)
    {
        // Deactivate all weapons in the weapon holder
        foreach (Transform weapon in weaponHolder)
        {
            weapon.gameObject.SetActive(false);
        }

        // Find the correct weapon and activate it
        Transform newWeapon = weaponHolder.Find(weaponName);
        if (newWeapon != null)
        {
            equippedWeaponObject = newWeapon.gameObject;
            equippedWeaponObject.SetActive(true);

            // Get weapon's collider (ensure the weapon has one!)
            weaponCollider = equippedWeaponObject.GetComponent<Collider>();
            if (weaponCollider == null)
            {
                Debug.LogError("No collider found on weapon: " + weaponName);
            }
            else
            {
                weaponCollider.enabled = false; // Make sure it's off initially
            }
            WeaponScript weaponScript = equippedWeaponObject.GetComponent<WeaponScript>();
            if (weaponScript != null)
        {
            weaponScript.currentWeapon = currentWeapon; // <-- THIS FIXES THE DAMAGE ISSUE
            Debug.Log("Weapon assigned: " + currentWeapon.weaponName + " Damage: " + currentWeapon.damage);
        }
        else
        {
            Debug.LogError("WeaponScript not found on " + weaponName);
        }
        }
        else
        {
            Debug.LogError("Weapon not found in WeaponHolder: " + weaponName);
        }
    }
    public void Attack()
    {
        if (currentWeapon != null)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown; // Set next available attack time
            if (attackAudioSource != null && attackAudioSource.clip != null)
            {
                attackAudioSource.Play();
                Debug.Log("Attack sound played!");
            }
            else
            {
                Debug.LogWarning("Attack sound not set or AudioSource missing!");
            }
            Debug.Log("Attacking with " + currentWeapon.weaponName + " - Damage: " + currentWeapon.damage);
            StartCoroutine(EnableWeaponColliderTemporarily());

        }
        else
        {
            Debug.Log("No weapon equipped!");
        }
    }
    private System.Collections.IEnumerator EnableWeaponColliderTemporarily()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true; // Enable collider
            yield return new WaitForSeconds(1f); // Keep it active briefly
            weaponCollider.enabled = false; // Disable again
        }
    }
    

}
