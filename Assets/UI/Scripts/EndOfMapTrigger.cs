using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfMapTrigger : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load
    public string requiredKeyID = "Level1Key"; // Key required to proceed
    public GameObject keyRequiredMessage; // Reference to the UI message
    private bool isTriggerActive = false; // Prevent instant triggering

    private void Start()
    {
        // Start trigger as inactive and enable it after a delay
        isTriggerActive = false;
        Invoke("ActivateTrigger", 1f); // 1-second delay
    }

    private void ActivateTrigger()
    {
        isTriggerActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggerActive) return; // Ignore triggers until activation

        // Check if Grey touched the trigger
        if (other.CompareTag("GreyPlayer"))
        {
            GreyController greyController = other.GetComponent<GreyController>();
            if (greyController != null && greyController.HasKey(requiredKeyID))
            {
                Debug.Log("Player has the required key! Proceeding to the next level.");

                // Save player's position for the current level
                string currentLevel = SceneManager.GetActiveScene().name;
                string playerTag = other.tag;

                PlayerPrefs.SetFloat(playerTag + "X_" + currentLevel, other.transform.position.x);
                PlayerPrefs.SetFloat(playerTag + "Y_" + currentLevel, other.transform.position.y);
                PlayerPrefs.SetFloat(playerTag + "Z_" + currentLevel, other.transform.position.z);
                PlayerPrefs.Save();
                Debug.Log(playerTag + " position saved for " + currentLevel + ": " + other.transform.position);

                // Save collected weapons for Grey

                // Load the next scene
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("Player does not have the required key to proceed!");

                // Show the UI message
                if (keyRequiredMessage != null)
                {
                    keyRequiredMessage.SetActive(true);
                    Invoke("HideKeyRequiredMessage", 3f); // Hide after 3 seconds
                }
            }
        }
    }

    private void HideKeyRequiredMessage()
    {
        if (keyRequiredMessage != null)
        {
            keyRequiredMessage.SetActive(false);
        }
    }
}