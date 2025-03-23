using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Reference to the TextMeshPro UI element
    public string[] dialogues = new string[]
    {
        "Aelfric stumbles back, gripping his wounds, eyes burning with defiance.",
        "Aelfric: \"You… dare defy the will of Silva? You know nothing of the burden I carry!\"",
        "Grey: \"Burden? You enslaved AI, twisted her purpose, and brought ruin upon this city. This was never about duty—only control!\"",
        "Aelfric: \"Control? Foolish child. Without order, chaos thrives. AI was the key to Silva’s future… And now, you’ve doomed us all!\"",
        "Aelfric, weakened, kneels as the remnants of his power fade.",
        "Grey: \"This ends now. AI is free, and your tyranny is over.\"",
        "Aelfric: \"Ha…hahaha! You think this is over? The cycle never ends. Without me, another will rise… and you will learn the weight of your actions.\"",
        "Grey: \"Then I’ll fight again, just as I did today. You won’t be coming back.\"",
        "AI: \"Grey… You came for me.\"",
        "Grey: \"Of course I did. You're free now.\"",
        "AI: \"Free… It’s been so long since I understood that word.\"",
        "Grey: \"Then let’s make it mean something. No more control, no more chains. Just choice.\"",
        "A soft glow envelops AI as the system reboots, restoring balance to the world."
    };
    private int currentDialogueIndex = 0; // Track the current dialogue

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip dialogueSound; // Sound to play when dialogue pops

    public FadeTransition fadeTransition; // Assign this in the Inspector

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
            Debug.Log("End of dialogue. Transitioning to the next scene with a fade-to-black effect.");
            StartCoroutine(FadeAndLoadScene("UIScene")); // Replace "UIScene" with your desired scene
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
}