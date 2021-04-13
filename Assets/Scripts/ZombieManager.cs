using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;
using UnityEngine.UI;

public class ZombieManager : MonoBehaviour
{
    [Header("Zombie's view ranges")]
    public float attackRange;
    public float moveRange;
    public float hearingRange;
    public float distance;
    public float view;

    [Header("Start stats")]
    float startSpeed;
    Transform startPosition;

    [Header("Zombie's health")]
    public int health;
    public bool isAlive;
    Collider2D c2d;
    public Action checkZombieHealth;
    public Slider healthBar;
    public Image sliderImage;

    [Header("Patrolling")]
    public GameObject[] patrolPoints;
    System.Random randomPatrolPoints;
    public float patrolSpeed;
    public float patrolChangeTime;

    [Header("Sounds & Effects")]
    SoundManager sM;
    public AudioClip moveSound;
    public AudioClip attackSound;

    [Header("Dependencies")]
    AIPath aiPath;
    AIDestinationSetter destinationSetter;
    Rigidbody2D rb;
    Animator animator;
    PlayerManager player;
    PlayerHealth playerHealth;
    ZombieState aciveState;

    enum ZombieState
    {
        ATTACK,
        MOVEtoPLAYER,
        PATROL,
        STAND
    }

    private void Awake()
    {
        startPosition = patrolPoints[0].transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        sM = FindObjectOfType<SoundManager>();
        aciveState = ZombieState.STAND;
        c2d = GetComponent<Collider2D>();
    }
    private void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        isAlive = true;
        player = FindObjectOfType<PlayerManager>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        randomPatrolPoints = new System.Random();
        StartCoroutine(ZombiePatrol());
    }
    void Update()
    {
        UpdateState();
        ZombieIsDead();

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
    void UpdateState()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        switch (aciveState)
        {
            case ZombieState.ATTACK:
                if (distance > attackRange && distance <= hearingRange)
                {
                    ChangeState(ZombieState.MOVEtoPLAYER);
                }

                break;

            case ZombieState.MOVEtoPLAYER:
                
                if (distance <= attackRange)
                {
                    ChangeState(ZombieState.ATTACK);
                }
                else if (distance > moveRange)
                {
                    ChangeState(ZombieState.PATROL);
                }
                break;

            case ZombieState.STAND:
                if (CheckMove())
                {
                    ChangeState(ZombieState.MOVEtoPLAYER);


                }
                break;
            case ZombieState.PATROL:
                if (CheckMove())
                {
                    ChangeState(ZombieState.MOVEtoPLAYER);
                }
                float distanceToPoint = Vector2.Distance(transform.position, patrolPoints[0].transform.position);
                if (distanceToPoint <= 0.01f)
                {
                    ChangeState(ZombieState.STAND);

                }
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
            rb.velocity = Vector2.zero;
            aiPath.enabled = false;
            StartCoroutine(DestroyBody(3f));
        }
    }
    private void ChangeState(ZombieState newState)
    {
        aciveState = newState;
        switch (aciveState)
        {
            case ZombieState.ATTACK:
                animator.SetTrigger("Attack");
                aiPath.enabled = false;
                break;

            case ZombieState.MOVEtoPLAYER:
                aiPath.enabled = true;
                destinationSetter.target = player.transform;
                animator.SetFloat("Speed", aiPath.maxSpeed);
                break;

            case ZombieState.STAND:
                aiPath.enabled = false;
                break;

            case ZombieState.PATROL:
                aiPath.enabled = true;
                destinationSetter.target = startPosition;
                animator.SetFloat("Speed", patrolSpeed);
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
    bool CheckMove()
    {
        if (distance <= moveRange && distance > attackRange)
        {
            LayerMask layerMask = LayerMask.GetMask("Obstacle");
            Vector2 direction = player.transform.position - transform.position;
            float viewAngle = Vector3.Angle(-transform.up, direction);
            if (viewAngle > view / 2)
            {
                return false;
            }
            Debug.DrawRay(transform.position, direction, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);
            if (hit.collider == null)
            {
                return true;
            }
        }
        if (distance <= hearingRange && distance > attackRange)
        {
            ChangeState(ZombieState.MOVEtoPLAYER);
        }
       

        return false;
    }
    public void Attack()
    {
        if (distance <= attackRange)
        {
            player.checkPlayerHealth();
            playerHealth.health--;
        }
    }
    IEnumerator DestroyBody(float wait)
    {
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
    IEnumerator ZombiePatrol()
    {
        while (true)
        {
            yield return new WaitForSeconds(patrolChangeTime);
            if (destinationSetter.target != player.transform)
            {
                int point = randomPatrolPoints.Next(0, patrolPoints.Length);
                startPosition = patrolPoints[point].transform;
                ChangeState(ZombieState.PATROL);
            }
        }
    }
}
