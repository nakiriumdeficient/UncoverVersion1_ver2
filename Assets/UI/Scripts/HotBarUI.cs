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
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (i < weapons.Count)
            {
                weaponSlots[i].sprite = weapons[i].weaponIcon; // Set collected weapon icon
            }
            else
            {
                weaponSlots[i].sprite = emptySlotSprite; // Show empty slot
            }
        }
    }
}
