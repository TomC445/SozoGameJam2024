using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 10;
    public int totalHealth;
    public GameObject alive;
    public GameObject dead;
    public GameObject[] toDisable;

    public ParticleSystem woodParticleSystem;

    public ShipSoundManager shipSoundManager;

    public bool isDead = false;


    void Start()
    {
        dead.SetActive(false);
        totalHealth = health;
    }

    void Update()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            dead.SetActive(true);
            
            alive.SetActive(false);
            foreach (GameObject obj in toDisable)
            {
                obj.SetActive(false);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if(health > 0)
        {
            health -= amount;
        }
        woodParticleSystem.Play();
        shipSoundManager.PlayHit();
    }
}
