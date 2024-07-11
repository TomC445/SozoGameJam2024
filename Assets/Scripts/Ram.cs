using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : MonoBehaviour
{
    private Health health;
    
    void Start()
    {
        health = GetComponent<Health>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
            health.TakeDamage(1);
        } else if (collision.gameObject.CompareTag("Tutorial"))
        {
            //do nothing
        }
        else if (!collision.gameObject.CompareTag("Terrain"))
        {
            health.TakeDamage(1);
        } 
    }
}
