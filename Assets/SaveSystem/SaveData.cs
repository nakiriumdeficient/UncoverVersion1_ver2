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
    public List<string> collectedItems;
    public Vector3 savedPosition;
    public int hpUpgradeCount; // How many times Max HP has been upgraded
    // Add these fields for ObjectiveManager state
    public bool hasKey2;
    public bool hasKey3;

    public List<string> defeatedBosses;
}