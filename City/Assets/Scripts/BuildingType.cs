using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingType
{
    [SerializeField]
    private GameObject[] prefabs;

    public int size;

    public GameObject getPrefab()
    {
        return prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
    }
    public GameObject[] getPrefabs()
    {
        return prefabs;
    }

}