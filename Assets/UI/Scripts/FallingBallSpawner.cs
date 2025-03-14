using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;  // Assign your sphere prefab in the Inspector
    public Transform spawnPoint;   // Set the position where the ball will spawn
    public float spawnInterval = 2f; // Time interval between spawns

    void Start()
    {
        InvokeRepeating(nameof(SpawnBall), 0f, spawnInterval);
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        ball.AddComponent<FallingBall>(); // Add the FallingBall script dynamically
    }
}
