using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Pathfinding;

public class BossScript : MonoBehaviour
{
    [Header("Zombie's view ranges")]
    public float meleeAttackRange;
    public float rangedAttackRange;
    public float hearingRange;
    float distance;

    [Header("Zombie's health")]
    public int health;
    public bool isAlive;
    Collider2D c2d;
    public Action checkZombieHealth;
    public Slider healthBar;
    public Image sliderImage;

    [Header("Dependencies")]
    Rigidbody2D rb;
    Animator animator;
    PlayerManager player;
    PlayerHealth playerHealth;
    ZombieState aciveState;
    SoundManager sM;
    AIPath aiPath;
    AIDestinationSetter destinationSetter;
    enum ZombieState
    {
        RANGEDATTACK,
        MELEEATTACK,
        MOVEtoPLAYER,
        STAND
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sM = FindObjectOfType<SoundManager>();
        aciveState = ZombieState.STAND;
        c2d = GetComponent<Collider2D>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aciveState = ZombieState.STAND;
    }
    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        isAlive = true;
        player = FindObjectOfType<PlayerManager>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    
    void Update()
    {
        ZombieIsDead();
    }

    void UpdateState()
    {
        distance = Vector2.Distance(transform.position, destinationSetter.target.position);
        switch (aciveState)
        {
            case ZombieState.RANGEDATTACK:
                if (distance <= meleeAttackRange)
                {
                    ChangeState(ZombieState.MELEEATTACK);
                }
                if (distance > meleeAttackRange)
                {
                    ChangeState(ZombieState.MOVEtoPLAYER);
                }
                break;
            case ZombieState.MELEEATTACK:
                if (healthBar.value <= healthBar.maxValue / 2)
                {
                    if (distance <= rangedAttackRange && distance > meleeAttackRange)
                    {
                        ChangeState(ZombieState.RANGEDATTACK);
                    }
                }
                
                break;
            case ZombieState.MOVEtoPLAYER:
                if (distance <= meleeAttackRange)
                {
                    ChangeState(ZombieState.MELEEATTACK);
                }
                break;
            case ZombieState.STAND:
                if (distance <= hearingRange)
                {
                    ChangeState(ZombieState.MOVEtoPLAYER);
                }
                break;
            default:
                break;
        }
    }
    void ChangeState(ZombieState newState)
    {
        aciveState = newState;
        switch (aciveState)
        {
            case ZombieState.RANGEDATTACK:
                animator.SetTrigger("Attack");
                aiPath.enabled = false;
                break;
            case ZombieState.MELEEATTACK:
                animator.SetTrigger("Melee");
                aiPath.enabled = false;
                break;
            case ZombieState.MOVEtoPLAYER:
                animator.SetFloat("Speed", 1);
                aiPath.enabled = true;
                break;
            case ZombieState.STAND:
                aiPath.enabled = false;
                break;
            default:
                break;
        }
    }
    void ZombieIsDead()
    {
        if (health <= 0)
        {
            animator.SetBool("IsDead", true);
            isAlive = false;
            c2d.isTrigger = true;
            aiPath.enabled = false;
            rb.velocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            health--;
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
            Destroy(collision.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}
