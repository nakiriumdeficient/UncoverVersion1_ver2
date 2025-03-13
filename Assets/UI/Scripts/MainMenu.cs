using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        continueButton.SetActive(GameManager.Instance.SaveExists());
    }

    public void NewGame()
    {
        GameManager.Instance.DeleteSave();
        SceneManager.LoadScene("Level1"); // Change to your first level
    }

    public void ContinueGame()
    {
        if (GameManager.Instance.SaveExists())
        {
            GameManager.Instance.LoadGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}