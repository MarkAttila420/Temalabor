using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PedestrianSpawner : MonoBehaviour
{
    public BodyParts[] parts;
    public List<GameObject> pedestrians;
    private NavMeshTriangulation triangulation;

    public void spawnAllPerson(int numberToSpawn)
    {
        for (int i = 0; i < numberToSpawn; ++i)
        {
            triangulation = NavMesh.CalculateTriangulation();
            int index = Random.Range(0, triangulation.vertices.Length);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(triangulation.vertices[index], out hit, 0.00001f, 1))
            {

                GameObject pedestrian = new GameObject();
                pedestrian.name = "pedestrian" + i;

                var body = Instantiate(parts[0].getPrefab(), new Vector3(0, 0.091f, 0), Quaternion.Euler(0, 90, 0));
                var head = Instantiate(parts[1].getPrefab(), new Vector3(0, 0.039f, 0), Quaternion.Euler(0, 90, 0));
                var legs = Instantiate(parts[2].getPrefab(), new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0));

                body.transform.parent = pedestrian.transform;
                head.transform.parent = pedestrian.transform;
                legs.transform.parent = pedestrian.transform;

                pedestrian.transform.position = new Vector3(0, 0, 0);

                NavMeshAgent agent = pedestrian.AddComponent<NavMeshAgent>();
                NavMesh.SamplePosition(triangulation.vertices[index], out hit, 3f, 1);         
                agent.speed = Random.Range(0.1f,LSystem.speed);
                agent.Warp(hit.position);
                agent.enabled = true;
                agent.acceleration = 600;
                agent.autoBraking = false;
                agent.autoRepath = true;
                agent.autoTraverseOffMeshLink = false;
                agent.radius = 0.00001f;

                var movement = pedestrian.AddComponent<PedestrianMovement>();

                pedestrians.Add(pedestrian);
            }
        }
    }

    public void delete()
    {
        foreach(var pedestrian in pedestrians)
        {
            Destroy(pedestrian);
        }
        pedestrians.Clear();
    }
}
