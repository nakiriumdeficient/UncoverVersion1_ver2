using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSpecial : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public float panOffset = 2f; // How much the camera shifts when moving
    public float cameraDistance = 5f; // Adjust how close the camera is to the player
    public float cameraHeight = 2f; // Adjust camera height
    public float xRotation = 10f; // Adjust camera tilt (rotation on X-axis)

    private Vector3 defaultOffset;
    private CharacterController playerController;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player is not assigned!");
            return;
        }

        playerController = player.GetComponent<CharacterController>();

        // Store the default offset but normalize it for dynamic distance adjustments
        defaultOffset = (transform.position - player.position).normalized;

        // Apply X-axis rotation at the start
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void LateUpdate()
    {
        if (player == null || playerController == null) return;

        // Get player's horizontal movement direction
        float moveDirection = Input.GetAxisRaw("Horizontal");

        // Calculate target camera position with adjustable distance and height
        Vector3 targetPosition = player.position + defaultOffset * cameraDistance;
        targetPosition.y += cameraHeight; // Apply height adjustment

        if (moveDirection > 0) // Moving right
        {
            targetPosition.x += panOffset;
        }
        else if (moveDirection < 0) // Moving left
        {
            targetPosition.x -= panOffset;
        }

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Apply X-axis rotation (so the player can adjust tilt anytime)
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
