﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    public float speed;
    public float patrolSpeed;
    Rigidbody2D rb;
    ZombieManager zombie;
    Animator animator;
    public Vector3 targetPosition;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        zombie = GetComponent<ZombieManager>();
    }
   
    void Update()
    {
        if (zombie.isAlive)
        {
            Vector3 zombiePosition = transform.position;
            Vector3 direction = targetPosition - zombiePosition;
            Move(direction);
            Rotation(direction);
        }
    }
    void Move(Vector3 direction)
    {
        rb.velocity = direction.normalized * speed;
        animator.SetFloat("Speed", speed);
    }
    void Rotation(Vector3 direction)
    {
        transform.up = -direction;
    }
    
    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        animator.SetFloat("Speed", speed);
    }
}
