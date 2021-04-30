using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    public PlayerManager player;
    public Image sliderImage;
    public PlayerDataSO playerData;
    private void Start()
    {
        healthBar.maxValue = playerData.maxHealth;
        healthBar.value = playerData.health;
    }

    public void HealthReduce()
    {
        healthBar.value--;
        if (healthBar.value <= healthBar.maxValue / 2)
        {
            Color orange = new Color(1.0f, 0.64f, 0.0f);
            sliderImage.color = orange;
            if (healthBar.value <= healthBar.maxValue / 4)
            {
                sliderImage.color = Color.red;
            }
        }
    }
    
}
