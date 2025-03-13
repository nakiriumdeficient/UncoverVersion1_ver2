using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePickup : MonoBehaviour
{
    public int expAmount; // Amount of XP this pickup gives
    public GameObject pickupEffectPrefab; // Assign particle effect prefab
    public AudioClip pickupSound; // Assign a sound effect
    private AudioSource audioSource;
    private Collider pickupCollider;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource from the prefab
        pickupCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // Ensure the player has the "Player" tag
        {
            PlayerExperience playerExp = other.GetComponent<PlayerExperience>();

            if (playerExp != null)
            {
                GameManager.Instance.GainXP(expAmount);
                pickupCollider.enabled = false;

                // Spawn particle effect at XP pickup location
                if (pickupEffectPrefab != null)
                {
                    Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                }

                // Play sound effect
                if (pickupSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(pickupSound);

                    StartCoroutine(DestroyAfterSound()); // Wait before destroying
                }
                else
                {
                    Destroy(gameObject); // No sound? Destroy immediately
                }
            }
        }
    }
    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(pickupSound.length);
        Destroy(gameObject);
    }
}
