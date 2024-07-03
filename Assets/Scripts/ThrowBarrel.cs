using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBarrel : MonoBehaviour
{
    public float trajectoryHeight = 10;
    public float trajectoryStrength = 5;
    public Transform projectileOffset;
    public GameObject projectilePrefab;
    
    private Vector3 mousePosition;

    private Vector3[] segments;
    private int numSegments = 0;
    private int maxIterations = 10000;
    private int maxSegmentCount = 300;
    private float segmentStepModulo = 4f;

    private Vector3 lineTrajectory = Vector3.zero;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        SimulatePath(transform.gameObject, lineTrajectory, projectilePrefab.GetComponent<Rigidbody>().drag);
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer = LayerMask.GetMask("Terrain");
        if(Physics.Raycast(ray, out hit, 1000, layer))
        {
            //transform.position = hit.point;
            mousePosition = hit.point;
            Vector3 mouseTrajectory = transform.position - mousePosition;
            Vector3 projectileTrajectory = new Vector3(mouseTrajectory.x, trajectoryHeight, mouseTrajectory.z).normalized;
            float mouseStrength = new Vector3(mouseTrajectory.x, 0, mouseTrajectory.z).magnitude;
            lineTrajectory = projectileTrajectory * mouseStrength * trajectoryStrength;
        }
        
    }

    private void OnMouseUp()
    {
        Vector3 mouseTrajectory = transform.position - mousePosition;
        Vector3 projectileTrajectory = new Vector3(mouseTrajectory.x, trajectoryHeight, mouseTrajectory.z).normalized;
        float mouseStrength = new Vector3(mouseTrajectory.x, 0, mouseTrajectory.z).magnitude;

        GameObject projectile = Instantiate(projectilePrefab, projectileOffset.position, projectilePrefab.transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectileTrajectory * mouseStrength * trajectoryStrength, ForceMode.Impulse);
        lineTrajectory = Vector3.zero;
    }

    private void SimulatePath(GameObject obj, Vector3 forceDirection, float drag)
    {
        float timestep = Time.fixedDeltaTime;
        float stepDrag = 1 - drag * timestep;
        Vector3 velocity = forceDirection * timestep;
        Vector3 gravity = Physics.gravity * timestep * timestep;
        Vector3 position = obj.transform.position;

        if (segments == null || segments.Length != maxSegmentCount)
        {
            segments = new Vector3[maxSegmentCount];
        }

        segments[0] = position;
        numSegments = 1;

        for (int i = 0; i < maxIterations && numSegments < maxSegmentCount; i++)
        {
            velocity += gravity;
            velocity *= stepDrag;

            position += velocity;
            if (i % segmentStepModulo == 0)
            {
                segments[numSegments] = position;
                numSegments++;
            }

            if(position.y < -10)
            {
                segments[numSegments] = position;
                break;
            }
        }
        Draw();
    }

    private void Draw()
    {
        Color startColor = Color.white;
        Color endColor = Color.white;
        startColor.a = 1f;
        endColor.a = 1f;

        lineRenderer.transform.position = segments[0];
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        lineRenderer.positionCount = numSegments;
        for(int i = 0; i < numSegments; i++)
        {
            lineRenderer.SetPosition(i, segments[i]);
        }
    }
}
