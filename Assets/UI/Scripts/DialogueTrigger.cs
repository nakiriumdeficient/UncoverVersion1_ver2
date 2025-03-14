using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox; // Reference to the DialogueBox UI Panel
    public string dialogueText; // Text to display in the dialogue box
    public AudioSource openSoundSource; // Sound when the dialogue box opens
    public AudioSource closeSoundSource; // Sound when the dialogue box closes

    private bool isDialogueActive = false; // Track if the dialogue box is active
    private bool hasBeenTriggered = false; // Track if the dialogue has been triggered

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger and the dialogue hasn't been triggered yet
        if (other.CompareTag("GreyPlayer") && !hasBeenTriggered)
        {
            // Show the dialogue box and set the text
            dialogueBox.SetActive(true);
            dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = dialogueText;
            isDialogueActive = true; // Set dialogue as active
            hasBeenTriggered = true; // Mark the dialogue as triggered

            // Play the open sound
            if (openSoundSource != null && openSoundSource.clip != null)
            {
                openSoundSource.Play();
            }
            else
            {
                Debug.LogWarning("Open sound not set or AudioSource missing!");
            }
        }
    }

    private void Update()
    {
        // Check if the dialogue box is active
        if (isDialogueActive)
        {
            // Close the dialogue box when Escape or left mouse button is pressed
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) // 0 = left mouse button
            {
                CloseDialogue();
            }
        }
    }

    private void CloseDialogue()
    {
        // Hide the dialogue box
        dialogueBox.SetActive(false);
        isDialogueActive = false; // Set dialogue as inactive

        // Play the close sound
        if (closeSoundSource != null && closeSoundSource.clip != null)
        {
            closeSoundSource.Play();
        }
        else
        {
            Debug.LogWarning("Close sound not set or AudioSource missing!");
        }
    }
}