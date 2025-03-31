using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    [Header("Core References")]
    public GameObject pauseMenuUI;
    public GameObject journalUI;
    public Transform player;
    public string playerTag = "GreyPlayer";

    [Header("Journal UI (TextMeshPro)")]
    public TMP_Text enemyNameText;
    public Image enemyImage;
    public TMP_Text enemyDescriptionText;
    public Button nextButton;
    public Button previousButton;
    public TMP_Text pageCounterText; // Added page counter

    [Header("Journal Data")]
    public List<EnemyJournalEntry> enemyEntries = new List<EnemyJournalEntry>();
    
    private int currentJournalIndex = 0;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
        
        // Optional: Keyboard navigation in journal
        if (journalUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) NextEnemy();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) PreviousEnemy();
        }
    }

    private void HandleEscapeKey()
    {
        if (journalUI.activeSelf)
        {
            CloseJournal();
        }
        else if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        journalUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenJournal()
    {
        pauseMenuUI.SetActive(false);
        journalUI.SetActive(true);
        currentJournalIndex = 0;
        UpdateJournalDisplay();
    }

    public void CloseJournal()
    {
        journalUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void NextEnemy()
    {
        currentJournalIndex = (currentJournalIndex + 1) % enemyEntries.Count;
        UpdateJournalDisplay();
    }

    public void PreviousEnemy()
    {
        currentJournalIndex--;
        if (currentJournalIndex < 0) currentJournalIndex = enemyEntries.Count - 1;
        UpdateJournalDisplay();
    }

    private void UpdateJournalDisplay()
    {
        if (enemyEntries.Count == 0) return;

        var currentEntry = enemyEntries[currentJournalIndex];
        enemyNameText.text = currentEntry.enemyName;
        enemyImage.sprite = currentEntry.enemyImage;
        enemyDescriptionText.text = currentEntry.enemyDescription;

        // Update page counter
        if (pageCounterText != null)
        {
            pageCounterText.text = $"{currentJournalIndex + 1}/{enemyEntries.Count}";
        }

        // Toggle navigation buttons
        bool multipleEntries = enemyEntries.Count > 1;
        nextButton.gameObject.SetActive(multipleEntries);
        previousButton.gameObject.SetActive(multipleEntries);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UIScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

[System.Serializable]
public class EnemyJournalEntry
{
    public string enemyName;
    public Sprite enemyImage;
    [TextArea(3, 10)]
    public string enemyDescription;
    
    [Header("Optional")]
    public Color nameColor = Color.white; // For TMP color customization
    public bool isBoss = false;
}