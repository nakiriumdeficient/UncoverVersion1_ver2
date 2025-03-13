using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public float playerX, playerY, playerZ;
    public int level;
    public int experience;

    public SaveData(Vector3 position, int level, int experience)
    {
        playerX = position.x;
        playerY = position.y;
        playerZ = position.z;
        this.level = level;
        this.experience = experience;
    }
}
