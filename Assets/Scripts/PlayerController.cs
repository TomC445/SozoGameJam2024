using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public Vector3 movement;
    public float moveDiff;
    public float acceleration;
    public float maxMoveSpeed;
    public float decayRate;
    public float rotationSpeed;
    public GameObject playerModel;
    public ParticleSystem[] moveParticles;
    public GameObject cameraPivot;

    private Rigidbody playerRb;
    private float initialHeight;
    public float speed;

    public Health playerHealth;

    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        initialHeight = gameObject.transform.position.y;
    }

    void Update()
    {
        if (!playerHealth.isDead)
        {
            MovePlayer();
        }
    }

    void FixedUpdate()
    {
        //handle rb movement in FixedUpdate
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector3 movement)
    {
        if (movement.magnitude > 0)
        {
            
            speed = Mathf.Min(speed + acceleration * Time.fixedDeltaTime,maxMoveSpeed);
            float dirDiff = Vector3.Dot(playerRb.velocity.normalized, movement);
            if(dirDiff < 0)
            {
                speed = Mathf.Max(speed + dirDiff * maxMoveSpeed, 0);
            }
            playerRb.velocity = movement * speed;
        }
        else
        {
            speed = Mathf.Max(speed - decayRate * Time.fixedDeltaTime, 0);
            playerRb.velocity = speed * playerModel.transform.forward.normalized;
        } 
    }

    void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = cameraPivot.transform.forward;
        Vector3 right = cameraPivot.transform.right;
        movement = (forward * verticalInput + right * horizontalInput).normalized;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * rotationSpeed);

        }

        if (playerRb.velocity.magnitude != 0)
        {
            foreach (ParticleSystem particles in moveParticles)
            {
                particles.Play();
                var emission = particles.emission;
                emission.rateOverTime = 200 * (playerRb.velocity.magnitude / maxMoveSpeed);
            }
        }
        else
        {
            foreach (ParticleSystem particles in moveParticles)
            {
                particles.Stop();
            }
        }
    }
}
