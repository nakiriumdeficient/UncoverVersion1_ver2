using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float rightOffset = -5f;
    public float leftOffset = 5f;
    public float yOffset = -3f;
    public float smoothSpeed = 0.125f;
    private bool isFacingRight = true;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("CameraFollow: Player reference is missing!");
            return;
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            isFacingRight = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            isFacingRight = false;
        }
        Vector3 desiredPosition = new Vector3(
            player.position.x + (isFacingRight ? rightOffset : leftOffset),
            player.position.y + yOffset,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
    }
}