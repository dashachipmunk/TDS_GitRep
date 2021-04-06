using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosives : MonoBehaviour
{
    public float explosiveRadius;
    public int explosiveDamage;
    PlayerHealth health;
    ZombieManager zombieHealth;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        health = FindObjectOfType<PlayerHealth>();
        zombieHealth = FindObjectOfType<ZombieManager>();
    }

    void Damage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == 9)
            {
                health.health -= explosiveDamage;
                health.healthBar.value -= explosiveDamage;
            }
            else if (collider.gameObject.layer == 13)
            {
                zombieHealth.health -= explosiveDamage;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            animator.SetTrigger("Explode");
            Damage();
            StartCoroutine(WaitAnimation(0.3f));
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosiveRadius);

    }
    IEnumerator WaitAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
