using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class Patrol : MonoBehaviour
{
    private Vector3 startPosition;
    private float walkingSpeed = 1;
    private float range = 20;
    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkingSpeed;
        startPosition = this.transform.position;
    }


    void NewDestination(Vector3 targetPoint)
    {
        agent.SetDestination(targetPoint);
    }


    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            Vector3 destination = startPosition + new Vector3(Random.Range(-range, range),
                                                              0,
                                                              Random.Range(-range, range));

            Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position);
            float oryginalX = transform.rotation.x;
            float oryginalZ = transform.rotation.z;
            Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.0f * Time.deltaTime);
            finalRotation.x = oryginalX;
            finalRotation.z = oryginalZ;
            transform.rotation = finalRotation;

            NewDestination(destination);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}