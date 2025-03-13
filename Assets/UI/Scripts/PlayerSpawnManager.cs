using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public GameObject greyPlayer; // Reference to the Grey GameObject
    public Vector3 greyDefaultSpawnPosition; // Default spawn position for Grey

    private void Start()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        Debug.Log("[PlayerSpawnManager] Current level: " + currentLevel);

        if (greyPlayer != null)
        {
            string keyX = "GreyPlayerX_" + currentLevel;
            string keyY = "GreyPlayerY_" + currentLevel;
            string keyZ = "GreyPlayerZ_" + currentLevel;

            if (PlayerPrefs.HasKey(keyX))
            {
                float x = PlayerPrefs.GetFloat(keyX);
                float y = PlayerPrefs.GetFloat(keyY);
                float z = PlayerPrefs.GetFloat(keyZ);
                Vector3 savedPosition = new Vector3(x, y, z);

                Debug.Log("[PlayerSpawnManager] Saved position for " + currentLevel + ": " + savedPosition);
                greyPlayer.transform.position = savedPosition;
            }
            else
            {
                Debug.Log("[PlayerSpawnManager] No saved position found for " + currentLevel + ". Using default spawn position.");
                greyPlayer.transform.position = greyDefaultSpawnPosition;
            }
        }
        else
        {
            Debug.LogError("[PlayerSpawnManager] GreyPlayer reference is missing!");
        }
    }

    public void RespawnGreyPlayer()
    {
        if (greyPlayer != null)
        {
            CharacterController controller = greyPlayer.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            greyPlayer.transform.position = greyDefaultSpawnPosition;
            Debug.Log("[PlayerSpawnManager] Grey respawned at default position: " + greyDefaultSpawnPosition);

            if (controller != null) controller.enabled = true;
        }
        else
        {
            Debug.LogError("[PlayerSpawnManager] Grey player reference is missing!");
        }
    }
}