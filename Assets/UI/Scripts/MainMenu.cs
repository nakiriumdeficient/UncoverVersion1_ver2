using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Function for the "Start New" button
    public void StartGame()
    {
        // Clear all saved data for both the cube and Grey
        ClearAllSavedPositions();

        // Clear the saved levels for both the cube and Grey
        PlayerPrefs.DeleteKey("SavedLevel_Cube");
        PlayerPrefs.DeleteKey("SavedLevel_Grey");

        // ✅ Clear saved weapons
        ClearSavedWeapons();

        // ✅ Clear saved keys
        ClearSavedKeys();

        PlayerPrefs.Save();

        // Load the first level
        SceneManager.LoadScene("Level1"); // Replace "Level1" with your first level scene name
    }

    // Function to clear all saved positions for both the cube and Grey
    private void ClearAllSavedPositions()
    {
        // Loop through all possible levels and delete their saved positions
        for (int i = 1; i <= 40; i++) // Adjust the range based on the number of levels in your game
        {
            string levelKey = "Level" + i;

            // Clear cube's saved positions
            PlayerPrefs.DeleteKey("CubePlayerX_" + levelKey);
            PlayerPrefs.DeleteKey("CubePlayerY_" + levelKey);
            PlayerPrefs.DeleteKey("CubePlayerZ_" + levelKey);

            // Clear Grey's saved positions
            PlayerPrefs.DeleteKey("GreyPlayerX_" + levelKey);
            PlayerPrefs.DeleteKey("GreyPlayerY_" + levelKey);
            PlayerPrefs.DeleteKey("GreyPlayerZ_" + levelKey);
        }
        PlayerPrefs.Save();
        Debug.Log("Cleared all saved positions for both the cube and Grey.");
    }

    // ✅ Function to clear collected weapons
    private void ClearSavedWeapons()
    {
        // Clear the collected weapon count
        PlayerPrefs.DeleteKey("CollectedWeaponsCount");

        // Loop through and delete all saved weapons
        for (int i = 0; i < 10; i++) // Supports up to 10 weapons
        {
            PlayerPrefs.DeleteKey("CollectedWeapon_" + i);
        }

        // Clear individual weapon pickup flags
        

        PlayerPrefs.Save();
        Debug.Log("Cleared all collected weapons.");
    }

    // ✅ Function to clear saved keys
private void ClearSavedKeys()
{
    // List all explicit key IDs used in the game
    string[] explicitKeyIDs = { "Level1Key", "Level5Key" }; // Add more keys as needed

    // Loop through and delete all explicit keys
    foreach (string keyID in explicitKeyIDs)
    {
        PlayerPrefs.DeleteKey(keyID);
        Debug.Log("Deleted explicit key: " + keyID);
    }

    // Loop through all possible keys based on level numbers
    for (int i = 1; i <= 40; i++) // Adjust the range based on the number of levels in your game
    {
        string keyID = "Level" + i + "Key"; // Match the keyID format used in your game
        PlayerPrefs.DeleteKey(keyID);
        Debug.Log("Deleted potential key: " + keyID);
    }

    PlayerPrefs.Save();
    Debug.Log("Cleared all saved keys.");
}

    // Function for the "Continue" button
 public void ContinueGame()
{
    // Check if there is saved progress for the player
    if (PlayerPrefs.HasKey("SavedLevel_GreyPlayer"))
    {
        // Load the saved level
        string savedLevel = PlayerPrefs.GetString("SavedLevel_GreyPlayer", "Level1"); // Default to Level1 if no saved data
        SceneManager.LoadScene(savedLevel);
    }
    else
    {
        // No saved progress, start from Level1
        SceneManager.LoadScene("Level1");
    }
}

    // Function for the "Quit" button
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // Note: This won't work in the Editor. Use Debug.Log to test.
        Debug.Log("Game Quit");
    }
}