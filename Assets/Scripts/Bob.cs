using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    public float bobFrequency;
    public float bobAmplitude;

    private float oscillationTime = 0f;
    private float previousOscillationAngle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        oscillationTime += bobFrequency * Time.deltaTime;
        float newOscillationAngle = Mathf.Sin(oscillationTime) * bobAmplitude;
        float deltaAngle = newOscillationAngle - previousOscillationAngle;
        transform.Rotate(Vector3.forward, deltaAngle * Random.Range(0.75f,1.25f));
        previousOscillationAngle = newOscillationAngle;
    }
}
