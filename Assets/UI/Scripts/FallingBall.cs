using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBall : MonoBehaviour
{
    public float fallSpeed = 5f;  // Adjust speed as needed

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // Make sure your player has the "Player" tag
        {
            Debug.Log("Player is hit by the ball");
        }
    }
}
