using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Player's body")]
    public float speed;
    Rigidbody2D rb;
    Collider2D c2d;

    [Header("Player's health")]
    PlayerHealth health;
    public bool isAlive;

    Animator animator;
    PlayerShooter playerShooter;
    public PlayerDataSO playerData;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        c2d = GetComponent<Collider2D>();
        playerShooter = GetComponent<PlayerShooter>();
    }
    private void Start()
    {
        isAlive = true;
        health = FindObjectOfType<PlayerHealth>();
    }
    void FixedUpdate()
    {
        if (isAlive)
        {
            Move();
        }
    }
    void Update()
    {
        if (isAlive)
        {
            Rotation();
        }

        Death();
    }
    void Move()
    {
        float posX = Input.GetAxis("Horizontal");
        float posY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(posX, posY) * speed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }
    void Rotation()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
        transform.up = direction;
    }
    void Death()
    {
        if (playerData.health == 0)
        {
            animator.SetBool("IsDead", true);
            isAlive = false;
            c2d.isTrigger = true;
            rb.freezeRotation = true;
            rb.velocity = Vector2.zero;
            playerShooter.enabled = false;
            StartCoroutine(Wait(3f));
        }
    }
    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int index = SceneManager.GetActiveScene().buildIndex;
        playerData.health = playerData.maxHealth;
        playerData.bulletsNumber = playerShooter.maxBulletsNumber;
        SceneManager.LoadScene(index);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index + 1);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            playerData.health--;
            health.HealthReduce();
            Destroy(collision.gameObject);
        }
    }
}
