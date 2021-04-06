using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public Slider healthBar;
    public PlayerManager player;
    public Image sliderImage;

    private void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        player.checkPlayerHealth += HealthReduce;
    }

    void HealthReduce()
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
