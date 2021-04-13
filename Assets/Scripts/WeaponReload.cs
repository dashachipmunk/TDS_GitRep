using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public int addBullets;
    PlayerShooter player;
    SoundManager sm;
    public AudioClip clip;
    private void Awake()
    {
        sm = FindObjectOfType<SoundManager>();
    }
    void Start()
    {
        player = FindObjectOfType<PlayerShooter>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (player.bulletsNumber < player.maxBulletsNumber)
            {
                player.bulletsNumber += addBullets;
                sm.PlaySound(clip);
                StartCoroutine(Wait(0.04f));
                if (player.bulletsNumber > player.maxBulletsNumber)
                {
                    player.bulletsNumber = player.maxBulletsNumber;
                }
            }

        }
    }
    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
