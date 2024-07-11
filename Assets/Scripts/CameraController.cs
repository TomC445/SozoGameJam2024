using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    public GameObject border;
    
    void Update()
    {
        //rotate the camera anticlockwise
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, cameraSpeed * Time.deltaTime);
            border.transform.Rotate(Vector3.forward, cameraSpeed * Time.deltaTime);
        }
        //rotate the camera clockwise
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -cameraSpeed * Time.deltaTime);
            border.transform.Rotate(Vector3.forward, -cameraSpeed * Time.deltaTime);
        }
    }
}
