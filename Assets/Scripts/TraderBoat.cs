using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TraderBoat : MonoBehaviour
{
    public GameObject[] ports;
    public float distance;

    private NavMeshAgent agent;
    private GameObject target;
    private Health health;
    private Rigidbody boatRb;
    private int portNum = 0;
    private bool isOffloading = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        boatRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            target = ports[0];
            agent.SetDestination(target.transform.position);
        }
        if (!isOffloading && (transform.position - target.transform.position).magnitude < distance)
        {
            StartCoroutine(Offload());
        }

        if (health.isDead)
        {
            agent.isStopped = true;
            agent.enabled = false;
            boatRb.isKinematic = true;
            Destroy(this);
        }
    }

    IEnumerator Offload()
    {
        isOffloading = true;
        yield return new WaitForSeconds(Random.Range(4, 10));
        portNum = Random.Range(0, ports.Length-1);
        target = ports[portNum];
        
        agent.SetDestination(target.transform.position);
        isOffloading = false;
    }
}
