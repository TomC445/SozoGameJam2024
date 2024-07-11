using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject waterParticles;
    public GameObject smokeParticles;

    public bool isPlayerProjectile;
    public int damage = 1;

    public int areaSize;
    public float power;
    
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Quaternion up = Quaternion.LookRotation(Vector3.up, Vector3.up);
        Vector3 pos = contact.point;

        if(collision.gameObject.CompareTag("Water"))
        {
            Instantiate(waterParticles, pos, up);
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Terrain"))
        {
            TerrainManager terrMan = collision.gameObject.GetComponent<TerrainManager>();
            terrMan.DeformTerrain(pos, areaSize, power);
            Instantiate(smokeParticles, pos, rot);
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Enemy") && isPlayerProjectile)
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(damage);
            Instantiate(smokeParticles, pos, rot);
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Player") && !isPlayerProjectile)
        {
            Health playerHealth = GameObject.Find("Player").GetComponent<Health>();
            playerHealth.TakeDamage(damage);
            Instantiate(smokeParticles, pos, rot);
            Destroy(gameObject);
        }
    }
}
