using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyMovement : MonoBehaviour
{
    private Animator animator;
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 10f; // Jump force
    public float gravity = 0f; // Custom gravity
    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
        // Check if the player is on the ground
    }

    void Update()
    {
        animator.SetBool("isMoving", false);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -4f; // Reset vertical velocity when grounded
        }

        // Get horizontal input (A/D or Left/Right Arrow)
        float moveInputX = Input.GetAxis("Horizontal"); // X-axis movement (left/right)

        // Calculate movement direction (only X-axis)
        Vector3 move = transform.right * moveInputX;

        // Apply movement (only X-axis)
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Flip the character based on movement direction
        if (moveInputX > 0) // Moving right
        {
            transform.localScale = new Vector3(1, 1, 1); // Face right
            animator.SetBool("isMoving", true);
        }
        else if (moveInputX < 0) // Moving left
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
            animator.SetBool("isMoving", true);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
