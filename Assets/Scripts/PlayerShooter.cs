using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Bullets shot;
    public GameObject shotPos;

    [Tooltip("Fire frequency")]
    float timer;
    public float fireFrequency;

    [Tooltip("Bullets")]
    public int bulletsNumber;
    public int maxBulletsNumber;
    public TextMeshProUGUI bulletsText;

    [Tooltip("Sounds")]
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
        bulletsText.text = bulletsNumber.ToString();
    }
    void Shoot()
    {
        if (player.isAlive)
        {
            PlayerPrefs.SetInt("Bullets", bulletsNumber);
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (Input.GetButton("Fire1") && timer <= 0)
            {
                animator.SetTrigger("Shoot");
                if (bulletsNumber > 0)
                {
                    sM.PlaySound(shotSound);
                    Instantiate(shot, shotPos.transform.position, transform.rotation);
                    bulletsNumber--;
                    timer = fireFrequency;
                }
                else if (bulletsNumber <= 0)
                {
                    audioSource.Play();
                }

            }
        }
    }
}
