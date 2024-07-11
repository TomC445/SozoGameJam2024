using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject target;
    public GameObject cannon;
    public GameObject barrel;
    public float rotationSpeed = 5;
    public float distance = 10;
    public ShipSoundManager shipSound;
    public float projectileSpeed = 10f;
    public float projectileHeight = 1;
    public GameObject cannonball;
    public Transform projectileOffset;
    public bool aimingAtPlayer;

    private Coroutine aimCoroutine;

    void Start()
    {
        //if a target isn't manually set get the player
        if (!target)
        {
            target = GameObject.Find("Player");
        }
    }

    void Update()
    {
        //look at the player and aim and fire, stop this process if not looking at player
        LookAtPlayer();
        if (aimingAtPlayer)
        {
            if (aimCoroutine == null)
            {
                aimCoroutine = StartCoroutine(AimAndFire());
            }
        } else
        {
            if(aimCoroutine != null)
            {
                StopCoroutine(aimCoroutine);
                aimCoroutine = null;
            }
        }
    }

    void LookAtPlayer()
    {
        //my forward
        Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z).normalized;
        //my position
        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        //player position
        Vector2 playerPosition = new Vector2(target.transform.position.x, target.transform.position.z);

        Vector2 playerDirection = (position - playerPosition);
        float playerDistance = playerDirection.magnitude;
        playerDirection = playerDirection.normalized;

        //check if aiming at player
        if(Vector2.Dot(playerDirection, forward2D) > 0)
        {
            if (playerDistance < distance) { aimingAtPlayer = true; }
            else { aimingAtPlayer = false; }
            Vector3 directionToPlayer = new Vector3(playerDirection.x, 0, playerDirection.y);
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            
            cannon.transform.rotation = Quaternion.Slerp(cannon.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        } else
        {
            aimingAtPlayer = false;
        }
    }

    IEnumerator AimAndFire()
    {
        //launch a projectile at the player every 3 seconds
        while(true)
        {
            LaunchProjectile();
            yield return new WaitForSeconds(3);
        }
    }

    void LaunchProjectile()
    {
        //launch a projectile at the player
        Vector3 position = transform.position;
        Vector3 playerPosition = target.transform.position;
        Vector3 playerDirection = position - playerPosition;

        Vector3 projectileDirection = (-playerDirection.normalized + Vector3.up * projectileHeight) * projectileSpeed * Mathf.Sqrt(playerDirection.magnitude);

        GameObject projectile = Instantiate(cannonball, projectileOffset.position, cannonball.transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectileDirection, ForceMode.Impulse);
        shipSound.PlayShoot();
    }
}
