using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    //public float speed;
    Rigidbody2D rb;
    PlayerManager player;
    Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerManager>();
        animator = GetComponent<Animator>();
    }
   
    void Update()
    {
        //rb.velocity = -transform.up * speed;
        //Vector2 shotDirection = player.transform.position;
        //transform.position = Vector2.MoveTowards(transform.position, shotDirection, speed * Time.deltaTime);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
}
