using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { 
    Trader,
    Enemy,
    Tower
}

public class EnemyDeath : MonoBehaviour
{
    public EnemyType enemyType;

    private Health health;
    private GameManager gameManager;
    private bool removed = false;

    void Start()
    {
        health = GetComponent<Health>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //removes enemy entity from enemy list on death
        if (!removed)
        {
            if (health.isDead)
            {
                gameManager.RemoveEntity(gameObject);
                removed = true;
            }
        }
    }
}
