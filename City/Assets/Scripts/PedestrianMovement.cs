using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class PedestrianMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshTriangulation triangulation;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        triangulation = NavMesh.CalculateTriangulation();
    }

    private void Update()
    {
        if (!agent.hasPath)
        {
            int index = UnityEngine.Random.Range(0, triangulation.vertices.Length);
            NavMeshHit hit;
            NavMesh.SamplePosition(triangulation.vertices[index], out hit, 50, 1);
            agent.SetDestination(hit.position);
        }
    }
}
