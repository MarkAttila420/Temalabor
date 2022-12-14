using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public List<NavMeshSurface> surfaces;

    public void bake()
    {
        for(int i = 0; i< surfaces.Count; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
