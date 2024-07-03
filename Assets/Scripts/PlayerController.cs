using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public Vector3 movement;
    public float moveSpeed;
    public float rotationSpeed;
    public GameObject playerModel;
    public ParticleSystem[] moveParticles;

    private Rigidbody playerRb;
    private float initialHeight;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        initialHeight = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector3 movement)
    {
        playerRb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
    }

    void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        movement = (Vector3.forward * verticalInput + Vector3.right * horizontalInput).normalized;
        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * rotationSpeed);
            foreach(ParticleSystem particles in moveParticles)
            {
                particles.Play();
            }
        } else
        {
            foreach (ParticleSystem particles in moveParticles)
            {
                particles.Stop();
            }
        }
    }
}
