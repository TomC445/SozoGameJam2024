using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoat : MonoBehaviour
{
    public Transform centerPoint;
    public float range;
    public float distance;
    public float aggroDistance;
    public float falloffDistance;
    public float changeDirectionDistance;

    //private GameObject player;
    private NavMeshAgent agent;
    private GameObject target;
    private Rigidbody boatRb;
    private Health health;
    private bool isDisabled;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        boatRb = GetComponent<Rigidbody>();
        target = GameObject.Find("Player");
    }

    void Update()
    {
        if (!isDisabled)
        {
            //if the player is near, navigate to the player
            if ((agent.transform.position - target.transform.position).magnitude < aggroDistance)
            {
                agent.SetDestination(target.transform.position);
            }
            //else go to a random point
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(centerPoint.position, range, out point))
                {
                    agent.SetDestination(point);
                }
            }
            //if dead then stop navmesh operations
            if (health.isDead)
            {
                agent.isStopped = true;
                agent.enabled = false;
                boatRb.isKinematic = true;
                Destroy(this);
            }
        }
        
    }

    //Gets a random point for the enemy to navigate to.
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }


}
