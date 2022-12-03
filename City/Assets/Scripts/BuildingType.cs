using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingType
{
    [SerializeField]
    private GameObject[] prefabs;

    //Ez a fuggveny akkor hivodik meg, ha olyan epuletet helyezunk le, ami egy prefab-bol all.
    public GameObject getPrefab()
    {
        return prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
    }

    //Ez a fuggveny akkor hivodik meg, ha olyan epuletet helyezunk le, ami tobb prefab-bol all.
    public GameObject[] getPrefabs()
    {
        return prefabs;
    }

}
