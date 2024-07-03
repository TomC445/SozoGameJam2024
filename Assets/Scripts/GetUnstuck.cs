using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUnstuck : MonoBehaviour
{
    private Rigidbody playerRb;
    private bool isColliding;
    private Coroutine unstick;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Hit the floor");
            playerRb.useGravity = false;
        }

        if (collision.gameObject.CompareTag("Terrain"))
        {
            if(!isColliding)
            {
                isColliding = true;
                playerRb.useGravity = true;
                unstick = StartCoroutine(CheckExtendedCollision(collision));
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            if (isColliding)
            {
                isColliding = false;
                if (unstick != null)
                {
                    StopCoroutine(unstick);
                }
            }
        }
    }

    private IEnumerator CheckExtendedCollision(Collision collision)
    {
        yield return new WaitForSeconds(3);
        Vector3 normal = collision.contacts[0].normal;
        Debug.Log(normal);
        Debug.DrawRay(collision.contacts[0].point, -normal, Color.red, 2.0f);
        playerRb.AddForce(-normal * 5, ForceMode.Force);
        Debug.Log("hi there");
    }
}
