using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BodyParts
{
    [SerializeField]
    private GameObject[] prefabs;

    public GameObject getPrefab()
    {
        return prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
    }
}
