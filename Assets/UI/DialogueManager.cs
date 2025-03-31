using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Reference to the TextMeshPro UI element
    public string[] dialogues; // Assign dialogues in the Inspector or via another script
    private int currentDialogueIndex = 0; // Track the current dialogue

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip dialogueSound; // Sound to play when dialogue pops

    public FadeTransition fadeTransition; // Assign this in the Inspector
    public string nextSceneName = "UIScene"; // Default to main menu, but can be changed in Inspector

    private void Start()
    {
        // Display the first dialogue
        if (dialogues.Length > 0)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
            PlayDialogueSound(); // Play sound for the first dialogue
        }
    }

    private void Update()
    {
        // Check for left-click to progress the dialogue
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
            ProgressDialogue();
        }
    }

    private void ProgressDialogue()
    {
        // Move to the next dialogue
        currentDialogueIndex++;

        // Check if there are more dialogues
        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
            PlayDialogueSound(); // Play sound for each new dialogue
        }
        else
        {
            // End of dialogue, transition to the next scene with a fade-to-black effect
            Debug.Log("End of dialogue. Transitioning to scene: " + nextSceneName);
            StartCoroutine(FadeAndLoadScene(nextSceneName));
        }
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade to black
        if (fadeTransition != null)
        {
            yield return fadeTransition.FadeToBlack();
        }

        // Load the next scene
        SceneManager.LoadScene(sceneName);

        // Fade from black (optional, if you want to fade in the next scene)
        if (fadeTransition != null)
        {
            yield return fadeTransition.FadeFromBlack();
        }
    }

    private void PlayDialogueSound()
    {
        if (audioSource != null && dialogueSound != null)
        {
            audioSource.PlayOneShot(dialogueSound); // Play the sound effect
        }
    }

    // Optional: Method to set dialogues dynamically from another script
    public void SetDialogues(string[] newDialogues, string targetSceneName)
    {
        dialogues = newDialogues;
        nextSceneName = targetSceneName;
        currentDialogueIndex = 0; // Reset to first dialogue
        if (dialogues.Length > 0)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
            PlayDialogueSound();
        }
    }
}