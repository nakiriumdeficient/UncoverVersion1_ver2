using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string savedScene;
    public int playerHPcur;
    public int playerHPmax;
    public int playerXP;
    public int maxXP;
    public int playerLevel;
    public int upgradeOrb;
    public List<WeaponData> collectedWeapons;
    public Vector3 savedPosition;
}
