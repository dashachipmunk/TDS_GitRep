using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    Vector2 startPosition;

    [Header("Zombie's health")]
    public int health;
    public bool isAlive;
    Collider2D c2d;
    public Action checkZombieHealth;

    [Header("Patrolling")]
    public GameObject[] patrolPoints;
    System.Random randomPatrolPoints;

    [Header("Sounds & Effects")]
    SoundManager sM;
    public AudioClip moveSound;
    public AudioClip attackSound;

    public GameObject blood;

    [Header("Dependencies")]
    ZombieMove zombie;
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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        zombie = GetComponent<ZombieMove>();
        sM = FindObjectOfType<SoundManager>();
        aciveState = ZombieState.STAND;
        c2d = GetComponent<Collider2D>();
    }
    private void Start()
    {
        startSpeed = 0;
        startPosition = transform.position;
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
            checkZombieHealth();
            health--;
            ChangeState(ZombieState.MOVEtoPLAYER);
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
                    sM.PlaySound(moveSound);
                }

                break;

            case ZombieState.MOVEtoPLAYER:
                zombie.targetPosition = player.transform.position;
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

                    ChangeState(ZombieState.MOVEtoPLAYER);
                    sM.PlaySound(moveSound);

                }
                break;
            case ZombieState.PATROL:
                if (CheckMove())
                {
                    ChangeState(ZombieState.MOVEtoPLAYER);
                }
                float distanceToPoint = Vector2.Distance(transform.position, startPosition);
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
            zombie.enabled = false;
            StartCoroutine(DestroyBody(3f));
        }
    }
    private void ChangeState(ZombieState newState)
    {
        aciveState = newState;
        switch (aciveState)
        {
            case ZombieState.ATTACK:
                animator.Play("Attack");
                zombie.enabled = false;
                break;

            case ZombieState.MOVEtoPLAYER:
                zombie.enabled = true;
                break;

            case ZombieState.STAND:
                zombie.enabled = false;
                animator.SetFloat("Speed", startSpeed);
                break;

            case ZombieState.PATROL:
                zombie.targetPosition = startPosition;
                zombie.enabled = true;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
    bool CheckMove()
    {
        if (distance <= moveRange && distance > attackRange)
        {
            LayerMask layerMask = LayerMask.GetMask("Wall");
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
            print("i hear you");
            return true;
        }
        
        return false;
    }
    bool CheckMove()
    {

        if (distance <= moveRange && distance > attackRange)
        {
            LayerMask layerMask = LayerMask.GetMask("Wall");
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
            yield return new WaitForSeconds(10);
            if (zombie.targetPosition != player.transform.position)
            {
                zombie.speed = zombie.patrolSpeed;
                int point = randomPatrolPoints.Next(0, patrolPoints.Length-1);
                startPosition = patrolPoints[point].transform.position;
                ChangeState(ZombieState.PATROL);
            }
        }
    }
}
