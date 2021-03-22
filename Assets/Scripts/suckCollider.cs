using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suckCollider : MonoBehaviour
{
    Collider vacuumCollider;
    ParticleSystem vacuumParticles;
    AudioSource suckAudio;

    private void Awake()
    {
        vacuumCollider = GetComponent<Collider>();
        vacuumCollider.gameObject.SetActive(true);
        vacuumCollider.isTrigger = true;
        vacuumCollider.enabled = false;
        vacuumParticles = GetComponentInChildren<ParticleSystem>();
        suckAudio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (vacuumCollider.enabled && !suckAudio.isPlaying)
            suckAudio.Play();

        if (!vacuumCollider.enabled && suckAudio.isPlaying)
            suckAudio.Stop();
    }

    public void EnableSuckCollider()
    {
        vacuumCollider.enabled = true;
    }

    public void DisableSuckCollider()
    {
        vacuumCollider.enabled = false;
        if (vacuumParticles.isPlaying)
            vacuumParticles.Stop();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<suckable>())
        {
            col.gameObject.GetComponent<suckable>().Suck();
            if (vacuumParticles.isStopped)
                vacuumParticles.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (vacuumParticles.isPlaying)
            vacuumParticles.Stop();
        
    }

}
