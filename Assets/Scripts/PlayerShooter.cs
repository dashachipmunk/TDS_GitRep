using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Lean.Pool;
public class PlayerShooter : MonoBehaviour
{
    public Bullets shot;
    public GameObject shotPos;

    [Header("Fire frequency")]
    float timer;
    public float fireFrequency;

    [Header("Bullets")]
    public PlayerDataSO playerData;
    public int maxBulletsNumber;
    public TextMeshProUGUI bulletsText;

    [Header("Sounds")]
    SoundManager sM;
    public AudioClip shotSound;
    AudioSource audioSource;

    Animator animator;
    PlayerManager player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sM = FindObjectOfType<SoundManager>();
        audioSource = GetComponent<AudioSource>();

    }
    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }
    void Update()
    {
        Shoot();
        bulletsText.text = playerData.bulletsNumber.ToString();
    }
    void Shoot()
    {
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (Input.GetButton("Fire1") && timer <= 0)
        {
            animator.SetTrigger("Shoot");
            if (playerData.bulletsNumber > 0)
            {
                sM.PlaySound(shotSound);
                LeanPool.Spawn(shot, shotPos.transform.position, transform.rotation);
                //Instantiate(shot, shotPos.transform.position, transform.rotation);
                playerData.bulletsNumber--;
                timer = fireFrequency;
            }
            else if (playerData.bulletsNumber <= 0)
            {
                audioSource.Play();
            }

        }
    }
}
