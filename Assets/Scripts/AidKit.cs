﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKit : MonoBehaviour
{
    public int health;
    PlayerHealth player;
    void Awake()
    {
        player = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            player.health += health;
            player.healthBar.value += health;
            Destroy(gameObject);
        }
    }
}
