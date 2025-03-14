using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefab; // Assign the GreyPlayer prefab
    public Transform spawnPoint; // Assign the spawn location

    private GameObject currentPlayer;
    private CameraFollow cameraFollow;

    void Start()
    {

        cameraFollow = FindObjectOfType<CameraFollow>(); // Find the camera follow script
        if (GameManager.Instance.SaveExists())
        {
            return; // Prevent spawning if game is loading
        }
        SpawnPlayerIfNeeded();

    }

    void SpawnPlayerIfNeeded()
    {
        if (GameObject.FindGameObjectWithTag("GreyPlayer") == null)
        {
            SpawnPlayer(spawnPoint.position);
        }
    }

    public void SpawnPlayer(Vector3 position)
    {
        GameObject newPlayer = Instantiate(playerPrefab, position, Quaternion.identity);

        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(newPlayer.transform);
        }
    }


}
