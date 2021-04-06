using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   

    public int health = 100;
    public float fireRate;
    public GameObject bulletPrefab;
    public Transform shootPosition;


    float nextFire;

    Animator anim;

 

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            anim.SetTrigger("shoot");
            Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);
            nextFire = fireRate;
            
        }

        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

    }
    public void DoDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
