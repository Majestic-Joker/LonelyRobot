using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suckable : MonoBehaviour
{
    private bool gettingSucked = false;
    private float health = 100;
    private Rigidbody rb;
    private Collider col;
    private AudioSource deathSound;
    private int count = 0;

    [SerializeField]
    private float damageThreshold = 25;
    [SerializeField]
    private float suckSpeed = 1.0f;
    [SerializeField]
    private GameObject gameManager;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        deathSound = GetComponent<AudioSource>();
        gameManager = GameObject.FindWithTag("GameController");
    }


    private void FixedUpdate()
    {
        if (health < damageThreshold)
        {
            if(count < 1 && !deathSound.isPlaying)
            {
                deathSound.Play();
                count++;
            }

            Die();
        }

        
        if (gettingSucked)
        {
            health -= 1 * suckSpeed;
            transform.localScale = new Vector3(health / 100, health / 100, health / 100);
            gettingSucked = false;
        }
    }

    public void Suck()
    {
        gettingSucked = true;
    }

    private void Die()
    {

        if (deathSound.isPlaying)
        {
            //Do nothing
        }   
        else
        {
            gameManager.GetComponent<GameManager>().AddScore();
            col.enabled = false;
            Destroy(gameObject);
        }
            
    }
}
