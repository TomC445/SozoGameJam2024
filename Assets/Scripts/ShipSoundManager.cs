using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSoundManager : MonoBehaviour
{
    public AudioClip shoot;
    public AudioClip hit;

    public AudioSource audiosrc;
    void Start()
    {
        audiosrc = GetComponent<AudioSource>();
    }

    public void PlayShoot()
    {
        audiosrc.PlayOneShot(shoot);
    }

    public void PlayHit()
    {
        audiosrc.Stop();
        audiosrc.clip = hit;
        audiosrc.Play();
    }
}
