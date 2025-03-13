using UnityEngine;

public class KeyController : MonoBehaviour
{
    public string keyID = "Level1Key"; // Unique ID for this key
    
    private void Start()
    {
        // Check if the key has already been collected
        if (PlayerPrefs.GetInt(keyID, 0) == 1)
        {
            // If the key has been collected, destroy it
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer"))
        {
            // Notify the player that they have collected the key
            GreyController greyController = other.GetComponent<GreyController>();
            if (greyController != null)
            {
                greyController.CollectKey(keyID);
            }

            // Save the key's collected state
            PlayerPrefs.SetInt(keyID, 1); // 1 = collected, 0 = not collected
            PlayerPrefs.Save();

            // Destroy the key object
            Destroy(gameObject);
        }
    }
}