using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 30 * Time.deltaTime);
    }
}
