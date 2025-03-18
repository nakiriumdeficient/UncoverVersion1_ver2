using System.Collections;
using UnityEngine;

public class FallingBallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;  // Assign your sphere prefab in the Inspector
    public Transform spawnPoint;   // Set the position where the ball will spawn
    public float spawnInterval = 2f; // Time interval between spawns
    public AudioClip spawnSound;   // Assign a sound effect for spawning in the Inspector
    [Range(0f, 1f)] public float spawnSoundVolume = 0.5f; // Volume level (0 to 1)

    private AudioSource audioSource; // AudioSource component to play the sound

    void Start()
    {
        // Get or add an AudioSource component to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start spawning balls
        InvokeRepeating(nameof(SpawnBall), 0f, spawnInterval);
    }

    void SpawnBall()
    {
        // Instantiate the ball
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);

        // Add the FallingBall script dynamically
        ball.AddComponent<FallingBall>();

        // Play the spawn sound with adjusted volume
        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound, spawnSoundVolume);
        }
        else
        {
            Debug.LogWarning("Spawn sound not assigned!");
        }
    }
}