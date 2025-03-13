using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain_Script : MonoBehaviour
{
    private CharacterController controller;
    public Animator npc_Controller; // Reference to the NPC's Animator
    public Transform player; // Reference to the player's Transform
    public float moveSpeed = 3f; // Speed at which the NPC moves
    public float attackRange = 2f; // Range at which the NPC can attack
    public float detectionRange = 10f; // Range at which the NPC detects the player
    public float rotationSpeed = 5f; // Speed of NPC rotation

    private bool isAttacking = false; // Track if the NPC is attacking

    void Start()
    {
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();

        // Ensure the Animator component is assigned
        if (npc_Controller == null)
        {
            npc_Controller = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        // Calculate the distance between the NPC and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Move toward the player if not within attack range
            if (distanceToPlayer > attackRange)
            {
                MoveTowardPlayer();
            }
            else
            {
                // Stop moving and attack if within attack range
                StopMoving();
                Attack();
            }
        }
        else
        {
            // Stop moving and reset attack if the player is out of range
            StopMoving();
            ResetAttack();
        }
    }

    // Move the NPC toward the player using CharacterController
    private void MoveTowardPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Ensure the NPC doesn't move vertically

        // Move using CharacterController
        controller.Move(direction * moveSpeed * Time.deltaTime);

        // Smoothly rotate the NPC to face the player
        RotateTowardsPlayer(direction);

        // Set the Animator parameter for movement
        npc_Controller.SetBool("IsMoving", true);
        npc_Controller.SetBool("IsAttacking", false);
    }

    // Rotate the NPC smoothly towards the player
    private void RotateTowardsPlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
        }
    }

    // Stop the NPC's movement
    private void StopMoving()
    {
        npc_Controller.Play("Idle");
    }

    // Trigger the NPC's attack
    private void Attack()
    {
        if (!isAttacking)
        {
            npc_Controller.SetTrigger("IsAttacking");
            isAttacking = true;
        }
    }

    // Reset the attack state
    private void ResetAttack()
    {
        isAttacking = false;
        npc_Controller.ResetTrigger("IsAttacking");
    }
}
