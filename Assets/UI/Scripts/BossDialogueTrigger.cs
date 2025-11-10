using UnityEngine;
using UnityEngine.UI; // Required for working with UI Images

public class BossDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueImage; // Reference to the UI Image GameObject (your PNG dialogue)
    public AudioSource openSoundSource; // Sound when the dialogue image appears
    public AudioSource closeSoundSource; // Sound when the dialogue image disappears

    private bool isDialogueActive = false; // Track if the dialogue image is active
    private bool hasBeenTriggered = false; // Track if the dialogue has been triggered


    public bool isCaptain = false;
    public bool isSage = false;
    public bool isAelfric = false;

    private GameObject bossHP; // Reference to UI Prompt
    private Collider triggerCollider;
    public string bossID = "";

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
        //  If the boss is defeated, remove this dialogue trigger
        if (GameManager.Instance.defeatedBosses.Contains(bossID))
        {
            Destroy(gameObject);
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger and the dialogue hasn't been triggered yet
        if (other.CompareTag("GreyPlayer") && !hasBeenTriggered)
        {
            // Show the dialogue image
            dialogueImage.SetActive(true);
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
        
        else if (isSage == true)
        {
            bossHP = GameObject.FindObjectOfType<Canvas>().transform.Find("SageHPBar")?.gameObject;

            bossHP.SetActive(true);
            triggerCollider.enabled = false;
        }
        else if (isAelfric == true)
        {
            bossHP = GameObject.FindObjectOfType<Canvas>().transform.Find("AelfricHPBar")?.gameObject;

            bossHP.SetActive(true);

            triggerCollider.enabled = false;
        }

    }

    private void Update()
    {
        // Check if the dialogue image is active
        if (isDialogueActive)
        {
            // Close the dialogue image when Escape or left mouse button is pressed
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) // 0 = left mouse button
            {
                CloseDialogue();
            }
        }

    }

    private void CloseDialogue()
    {
        // Hide the dialogue image
        dialogueImage.SetActive(false);
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