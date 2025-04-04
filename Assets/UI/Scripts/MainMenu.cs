using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject settingspanel;

    private void Start()
    {
        continueButton.SetActive(GameManager.Instance.SaveExists());
    }

    public void NewGame()
    {
        SaveSystem.DeleteSave(); // Delete all saved data
        GameManager.Instance.ResetGameData(); // Reset player stats, inventory, orbs, etc.

        SceneManager.LoadScene("Level0"); // Change this to your starting scene
    }

    public void ContinueGame()
    {
        if (SaveSystem.SaveExists())
        {
            GameManager.Instance.LoadGame(); // Load all game data
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }
    public void SettingsON()
    {
        settingspanel.SetActive(true);
    }
    public void SettingsOFF()
    {
        settingspanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}