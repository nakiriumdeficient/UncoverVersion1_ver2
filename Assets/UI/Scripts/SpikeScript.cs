using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if Grey touched the spike
        if (other.CompareTag("GreyPlayer"))
        {
            Debug.Log("Grey touched a spike!");

            // Get the PlayerSpawnManager in the scene
            PlayerSpawnManager spawnManager = FindObjectOfType<PlayerSpawnManager>();
            if (spawnManager != null)
            {
                // Respawn Grey
                spawnManager.RespawnGreyPlayer();
            }
            else
            {
                Debug.LogError("PlayerSpawnManager not found in the scene!");
            }
        }
    }
}
