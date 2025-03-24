using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarUI : MonoBehaviour
{
    public List<Image> weaponSlots; // UI slot images
    public Sprite emptySlotSprite;  // Default icon for empty slots
    public Color activeColor = Color.white;   // Bright color for equipped weapon
    public Color inactiveColor = new Color(1, 1, 1, 0.5f);  // Dimmed color for others

    private void Start()
    {
        UpdateHotbar();
    }

    public void UpdateHotbar()
    {
        List<WeaponData> weapons = GameManager.Instance.collectedWeapons;
        string equippedWeaponName = FindObjectOfType<GreyAttack>().GetCurrentWeaponName(); // Get equipped weapon name

        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (i < weapons.Count)
            {
                weaponSlots[i].sprite = weapons[i].weaponIcon; // Show weapon icon
                weaponSlots[i].color = (weapons[i].weaponName == equippedWeaponName) ? activeColor : inactiveColor; // Bright or dim
            }
            else
            {
                weaponSlots[i].sprite = emptySlotSprite; // Show empty slot
                weaponSlots[i].color = inactiveColor; // Always dim empty slots
            }
        }
    }
}
