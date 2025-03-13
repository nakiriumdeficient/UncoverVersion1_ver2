using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingWeapon : MonoBehaviour
{
    public float bobSpeed = 2f; // Speed of bobbing
    public float bobAmount = 0.2f; // How much it moves up/down

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Store the initial position
    }

    void Update()
    {
        float newY = startPos.y + Mathf.PingPong(Time.time * bobSpeed, bobAmount * 2) - bobAmount;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        transform.Rotate(Vector3.up * 30f * Time.deltaTime);
    }
}
