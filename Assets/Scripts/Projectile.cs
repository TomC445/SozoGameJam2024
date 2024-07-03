using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject waterParticles;
    public GameObject smokeParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            Instantiate(smokeParticles, pos, rot);
            Destroy(gameObject);
        }
        
    }
}
