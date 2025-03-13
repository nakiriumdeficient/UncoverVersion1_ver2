using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill; // Reference to the fill image
    public TextMeshProUGUI hptext;
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.enabled = health > 0;
        hptext.text = $"{GameManager.Instance.playercurHP} / {GameManager.Instance.playermaxHP}";
    }
}
