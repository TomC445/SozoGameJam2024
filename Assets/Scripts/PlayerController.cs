using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float moveSpeed;
    public float rotationSpeed;
    public GameObject playerModel;
    public ParticleSystem[] moveParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = Vector3.forward * verticalInput + Vector3.right * horizontalInput;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        if (horizontalInput != 0 || verticalInput != 0)
        {
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotationSpeed);
            foreach(ParticleSystem particles in moveParticles)
            {
                particles.Play();
            }
        } else
        {
            foreach (ParticleSystem particles in moveParticles)
            {
                particles.Stop();
                Debug.Log("Stopped particles");
            }
        }
    }
}
