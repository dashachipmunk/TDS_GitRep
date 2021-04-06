using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ZombieUI : MonoBehaviour
{
    ZombieManager zombie;
    Slider slider;
    public Image sliderImage;
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        zombie = GetComponentInParent<ZombieManager>();
        slider.maxValue = zombie.health; 
        slider.value = zombie.health;
        zombie.checkZombieHealth += UpdateHealthSlider;
    }
    void UpdateHealthSlider()
    {
        slider.value--;
        if (slider.value <= slider.maxValue / 2)
        {
            Color orange = new Color(1.0f, 0.64f, 0.0f);
            sliderImage.color = orange;
            if (slider.value <= slider.maxValue / 4)
            {
                sliderImage.color = Color.red;
            }
        }
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
