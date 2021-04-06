using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float followDistance;
    public float attackDistance;
    Animator anim;
    float distanse;

    Player player;
    ZombieMove movement;


    ZombieState activeState;

    enum ZombieState
    {
        STAND,
        MOVE,
        ATTACK
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<ZombieMove>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        activeState = ZombieState.STAND;
        
    }

    // Update is called once per frame
    void Update()
    {


        distanse = Vector3.Distance(transform.position, player.transform.position);
        

       
       
        switch (activeState)
        {
            case ZombieState.STAND:
          
                ChangeStand(distanse);
                movement.enabled = false;

                break;
            case ZombieState.MOVE:
                
                ChangeStand(distanse);
                movement.enabled = true;
                break;
            case ZombieState.ATTACK:
                anim.SetTrigger("shoot");
                movement.enabled = false;
                Rotate();
                ChangeStand(distanse);
                break;
        }

    }

    private void ChangeStand(float distanse)
    {
        if (distanse <= followDistance && distanse > attackDistance)
        {
            activeState = ZombieState.MOVE;
        }
        else if (distanse <= attackDistance)
        {
            activeState = ZombieState.ATTACK;
        }
        else
        {
            activeState = ZombieState.STAND;
        }
    }
    void Rotate()
    {
        Vector3 zombiePoition = transform.position;
        Vector3 playerPosition = player.transform.position;

        Vector3 direction = playerPosition - zombiePoition;

        direction.z = 0;
        transform.up = -direction;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }

    public void DoDamage()
    {
        if (distanse < attackDistance)
        {
            print("Damage");
        }
    }
    }
