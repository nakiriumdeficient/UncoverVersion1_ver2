using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionZone : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load (forward)
    public string previousSceneName; // Name of the previous scene to load (downward)
    private bool isPlayerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if Grey entered the zone
        if (other.CompareTag("GreyPlayer"))
        {
            isPlayerInZone = true;
            Debug.Log("Press W to proceed forward or S to go downward.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if Grey exited the zone
        if (other.CompareTag("GreyPlayer"))
        {
            isPlayerInZone = false;
            Debug.Log("Left the transition zone.");
        }
    }

    private void Update()
    {
        if (isPlayerInZone)
        {
            // Transition forward (next scene) when W is pressed
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!string.IsNullOrEmpty(nextSceneName))
                {
                    SavePlayerData(SceneManager.GetActiveScene().name, "GreyPlayer");
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.LogWarning("Next scene name is not set. Cannot transition forward.");
                }
            }

            // Transition downward (previous scene) when S is pressed
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!string.IsNullOrEmpty(previousSceneName))
                {
                    SavePlayerData(SceneManager.GetActiveScene().name, "GreyPlayer");
                    SceneManager.LoadScene(previousSceneName);
                }
                else
                {
                    Debug.LogWarning("Previous scene name is not set. Cannot transition downward.");
                }
            }
        }
    }

    // Helper method to save the player's position and weapons
    private void SavePlayerData(string currentLevel, string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            // Save the player's position based on their tag
            PlayerPrefs.SetFloat(playerTag + "X_" + currentLevel, player.transform.position.x);
            PlayerPrefs.SetFloat(playerTag + "Y_" + currentLevel, player.transform.position.y);
            PlayerPrefs.SetFloat(playerTag + "Z_" + currentLevel, player.transform.position.z);
            PlayerPrefs.Save();

            Debug.Log(playerTag + " position saved for " + currentLevel + ": " + player.transform.position);

            // Save collected weapons for Grey
            GreyController greyController = player.GetComponent<GreyController>();
            if (greyController != null)
            {
                
            }
        }
        else
        {
            Debug.LogError("Player not found with tag: " + playerTag);
        }
    }
}