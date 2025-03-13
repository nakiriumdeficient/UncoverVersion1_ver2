using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the Pause Menu UI Panel
    public Transform player; // Reference to the player's transform
    public string playerTag = "GreyPlayer"; // Tag of the player (e.g., "GreyPlayer")

    private bool isPaused = false;

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        // Save the player's position before loading the main menu
        SavePlayerPosition();

        Time.timeScale = 1f; // Reset time scale before loading the menu
        SceneManager.LoadScene("UIScene"); // Replace "UIScene" with your main menu scene name
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game
    }

    private void SavePlayerPosition()
    {
        if (player == null)
        {
            Debug.LogError("Player transform not set in PauseMenu!");
            return;
        }

        // Get the current level name
        string currentLevel = SceneManager.GetActiveScene().name;

        // Save the player's position
        PlayerPrefs.SetFloat(playerTag + "X_" + currentLevel, player.position.x);
        PlayerPrefs.SetFloat(playerTag + "Y_" + currentLevel, player.position.y);
        PlayerPrefs.SetFloat(playerTag + "Z_" + currentLevel, player.position.z);

        // Save the current level
        PlayerPrefs.SetString("SavedLevel_" + playerTag, currentLevel);

        PlayerPrefs.Save();
        Debug.Log("Saved player position for " + currentLevel + ": " + player.position);
    }
}