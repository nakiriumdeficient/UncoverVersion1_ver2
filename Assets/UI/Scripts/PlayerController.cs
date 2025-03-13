using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed for horizontal movement
    public float jumpForce = 10f; // Force applied when jumping
    private bool isGrounded; // Check if the player is on the ground

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void Update()
    {
        // Horizontal movement (A for left, D for right)
        float moveInput = Input.GetAxis("Horizontal"); // Returns -1 (A), 0, or 1 (D)
        rb.velocity = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0); // Move only on the X-axis

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0); // Apply jump force
        }
    }

    // Check if the player is on the ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}