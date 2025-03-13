using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreyController : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f; // Jump speed variable
    public float gravity = 20.0f;
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    private Transform modelTransform;
    private bool facingRight = true;
    private bool isJumping = false;

    private bool isAttacking = false;
    private bool isDead = false;
    private PlayerSpawnManager spawnManager;

    // Weapon system variables
    

    // Audio
    public AudioSource moveAudioSource; // Reference to the AudioSource for moving sounds
    private bool isMovingSoundPlaying = false; // Track if the moving sound is playing

    // Key system variables
    private bool hasKey = false; // Track if the player has collected the key

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); // Get Animator from child
        modelTransform = animator.transform;
        spawnManager = FindObjectOfType<PlayerSpawnManager>();

        if (animator == null)
        {
            Debug.LogError("Animator component missing from Grey or its children!");
        }

        if (spawnManager == null)
        {
            Debug.LogError("PlayerSpawnManager not found in scene!");
        }

        // Initialize weapons
        

        // Initialize move audio source
        if (moveAudioSource == null)
        {
            Debug.LogError("Move AudioSource not set!");
        }
    }

    void Update()
    {
        if (isDead) return; // Prevent movement when dead

        float moveHorizontal = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(moveHorizontal) > 0;

        // Handle moving sound
        if (isMoving)
        {
            if (!isMovingSoundPlaying)
            {
                moveAudioSource.Play(); // Play the moving sound
                isMovingSoundPlaying = true;
            }
        }
        else
        {
            if (isMovingSoundPlaying)
            {
                moveAudioSource.Stop(); // Stop the moving sound
                isMovingSoundPlaying = false;
            }
        }

        if (controller.isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
                animator.ResetTrigger("Jump");
            }

            moveDirection = new Vector3(moveHorizontal, 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButtonDown("Jump") && controller.isGrounded) // Jump input
            {
                moveDirection.y = jumpSpeed; // Apply jump speed
                animator.SetTrigger("Jump");
                isJumping = true;
                Debug.Log("Jump triggered!");
            }
        }

        moveDirection.y -= gravity * Time.deltaTime; // Apply gravity
        controller.Move(moveDirection * Time.deltaTime); // Move the player

        
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }

        if (moveHorizontal > 0 && !facingRight) // Rotate character to face right
        {
            RotateCharacter(90);
        }
        else if (moveHorizontal < 0 && facingRight) // Rotate character to face left
        {
            RotateCharacter(-90);
        }
    }

    void RotateCharacter(float yRotation)
    {
        facingRight = !facingRight;
        modelTransform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    

    

    // Weapon system methods
    

    

    // Save collected weapons to PlayerPrefs
    

    // Load collected weapons from PlayerPrefs
    

    // Key system methods
    public void CollectKey(string keyID)
    {
        hasKey = true;
        PlayerPrefs.SetInt(keyID, 1); // Save key state
        PlayerPrefs.Save();
        Debug.Log("Key collected: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return PlayerPrefs.GetInt(keyID, 0) == 1; // Check if key is collected
    }
}