using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    public float bobFrequency;
    public float bobAmplitude;

    private float oscillationTime = 0f;
    private float previousOscillationAngle = 0f;

    void Update()
    {
        //bobs objects back and forth
        oscillationTime += bobFrequency * Time.deltaTime;
        float newOscillationAngle = Mathf.Sin(oscillationTime) * bobAmplitude;
        float deltaAngle = newOscillationAngle - previousOscillationAngle;
        transform.Rotate(Vector3.forward, deltaAngle);
        previousOscillationAngle = newOscillationAngle;
    }
}
