using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerManager player;
    public float speed;

    [Header("Shots params")]
    public GameObject shotPos;
    public float fireFrequency;
    public Shots shot;

    [Header("Enemy's health")]
    public int health;
    bool isAlive;

    
   // Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }
    private void Start()
    {
        isAlive = true;
        player = FindObjectOfType<PlayerManager>();
        //shot = FindObjectOfType<Shots>();
        StartCoroutine(EnemyShoots(fireFrequency));
    }
    void Update()
    {
        if (isAlive)
        {
            Vector2 enemyPosition = transform.position;
            Vector2 playerPosition = player.transform.position;
            Vector2 direction = playerPosition - enemyPosition;
            Move(direction);
            Rotation(direction);
        }
        if (health <= 0)
        {
           //animator.SetBool("IsDead", true);
            isAlive = false;
        }
    }
    void Move(Vector2 direction)
    {
        rb.velocity = direction * speed;
       // animator.SetFloat("Speed", rb.velocity.magnitude);

    }
    void Rotation(Vector2 direction)
    {
        transform.up = -direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            health--;
            Destroy(collision.gameObject);
        }
    }
    IEnumerator EnemyShoots(float shootTime)
    {
        while (isAlive)
        {
            //animator.SetTrigger("Shoot");
            Instantiate(shot, shotPos.transform.position, transform.rotation);
            yield return new WaitForSeconds(shootTime);
        }

    }
}
