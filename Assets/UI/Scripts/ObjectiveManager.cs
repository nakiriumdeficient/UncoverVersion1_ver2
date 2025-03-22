using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance; // Singleton pattern for easy access

    private TextMeshProUGUI ObjectiveIndicator; // Reference to the TextMeshPro UI element
    public bool hasKey2 = false; // Track if Key2 is obtained
    public bool hasKey3 = false; // Track if Key3 is obtained
    private bool hasEnteredLevel28 = false; // Track if the player has entered Level 28

    private void Awake()
    {
        // Ensure only one instance of ObjectiveManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persistent
            Debug.Log("ObjectiveManager initialized and set as Instance.");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.Log("Duplicate ObjectiveManager destroyed.");
        }
    }

    private void Start()
    {
        // Find the ObjectiveIndicator in the initial scene
        FindObjectiveIndicator();
        UpdateObjective();

        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the player has entered Level 28
        int currentLevel = GetCurrentLevelNumber();
        if (currentLevel == 28)
        {
            hasEnteredLevel28 = true;
        }

        // Find the ObjectiveIndicator in the new scene
        FindObjectiveIndicator();
        UpdateObjective();
    }

    private void FindObjectiveIndicator()
    {
        // Find the ObjectiveIndicator in the scene
        GameObject objectiveIndicatorObject = GameObject.Find("ObjectiveIndicator"); // Replace with the correct name
        if (objectiveIndicatorObject != null)
        {
            ObjectiveIndicator = objectiveIndicatorObject.GetComponent<TextMeshProUGUI>();
            if (ObjectiveIndicator == null)
            {
                Debug.LogError("ObjectiveIndicator GameObject does not have a TextMeshProUGUI component!");
            }
            else
            {
                Debug.Log("ObjectiveIndicator found and assigned.");
            }
        }
        else
        {
            Debug.Log("ObjectiveIndicator not found in this scene. This is expected in the main menu.");
            ObjectiveIndicator = null; // Set to null to avoid errors
        }
    }

    // Make this method public so it can be called from other scripts
    public void UpdateObjective()
    {
        if (ObjectiveIndicator == null)
        {
            Debug.Log("ObjectiveIndicator is null. This is expected in the main menu.");
            return;
        }

        // Get the current level number from the scene name
        int currentLevel = GetCurrentLevelNumber();

        if (currentLevel == 33) // Aelfric Castle is Level 33
        {
            // Level 33: Final battle
            ObjectiveIndicator.text = "Confront Aelfric and save AI.";
        }
        else if (hasEnteredLevel28) // Player has entered Level 28
        {
            // Main objective is now finding the key to Aelfric's Castle
            if (!hasKey3)
            {
                ObjectiveIndicator.text = "Find the key to unlock Aelfric Castle.";
            }
            else
            {
                ObjectiveIndicator.text = "You have the key! Proceed to Aelfric Castle (Level 33).";
            }
        }
        else if (currentLevel >= 2 && currentLevel <= 11) // Levels 2–11: Enter Silva Forest to find the key
        {
            ObjectiveIndicator.text = "Enter Silva Forest to find the key.";
        }
        else if (currentLevel >= 12 && currentLevel <= 22) // Levels 12–22: Find the key to open the gate
        {
            if (!hasKey2)
            {
                ObjectiveIndicator.text = "Find the key to open the gate.";
            }
            else
            {
                ObjectiveIndicator.text = "Use the key to open the gate.";
            }
        }
        else
        {
            // Default objective for other levels
            ObjectiveIndicator.text = "Proceed to the next level.";
        }
    }

    // Helper method to get the current level number from the scene name
    private int GetCurrentLevelNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level"))
        {
            string levelNumberString = sceneName.Replace("Level", "");
            if (int.TryParse(levelNumberString, out int levelNumber))
            {
                return levelNumber;
            }
        }
        return -1; // Default value if the scene name doesn't match the expected format
    }

    // Call this method when an item is collected
    public void CollectItem(string itemName)
    {
        if (itemName == "Key2")
        {
            hasKey2 = true;
        }
        else if (itemName == "Key3")
        {
            hasKey3 = true;
        }
        UpdateObjective();
    }
}