using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehavior : MonoBehaviour
{
    public AudioClip destroySound; // Sound to play when the crate is destroyed (optional)
    public ParticleSystem destroyEffect; // Particle effect to play when the crate is destroyed (optional)
    public float destroyDelay = 1f; // Delay before destroying the crate (optional)

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Crate detected collision with: " + other.gameObject.name); // Log what entered the trigger

        // Check if the collider has the WeaponScript component
        WeaponScript weapon = other.GetComponent<WeaponScript>();
        if (weapon != null) // If the collider has the WeaponScript component
        {
            Debug.Log("Crate hit by weapon!");

            // Play sound effect (if assigned)
            if (destroySound != null)
            {
                AudioSource.PlayClipAtPoint(destroySound, transform.position);
            }

            // Play particle effect (if assigned)
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }

            // Start the destruction process
            StartCoroutine(DestroyCrate());
        }
    }

    private IEnumerator DestroyCrate()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the crate (or its parent CrateObject)
        if (transform.parent != null)
        {
            Debug.Log("Destroying crate parent: " + transform.parent.name);
            Destroy(transform.parent.gameObject); // Destroy the parent if it exists
        }
        else
        {
            Debug.Log("Destroying crate directly: " + gameObject.name);
            Destroy(gameObject); // Destroy the crate directly if it has no parent
        }
    }
}