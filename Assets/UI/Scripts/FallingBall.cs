using UnityEngine;

public class FallingBall : MonoBehaviour
{
    public float fallSpeed = 5f;  // Adjust speed as needed

    void Update()
    {
        // Move the ball downward
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the ball collided with the player
        if (other.CompareTag("GreyPlayer")) // Make sure your player has the "GreyPlayer" tag
        {
            Debug.Log("Player is hit by the ball");

            // Call the GameManager to reload the game
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadGame();
            }
            else
            {
                Debug.LogError("GameManager instance is null! Make sure the GameManager is set up correctly.");
            }
        }
    }
}