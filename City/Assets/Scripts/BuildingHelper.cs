using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHelper : MonoBehaviour
{
    public GameObject[] smallHouses;
    public Dictionary<Vector3Int,GameObject> dictionary=new Dictionary<Vector3Int,GameObject>();

    public void placeBuildings(List<Vector3Int> roads)
    {
        Dictionary<Vector3Int, Direction> freeSpots = FindFreeSpots(roads);
        foreach (var pos in freeSpots.Keys)
        {
            Instantiate(smallHouses[UnityEngine.Random.Range(0,smallHouses.Length)],pos,Quaternion.identity,transform);
        }
    }

    private Dictionary<Vector3Int, Direction> FindFreeSpots(List<Vector3Int> roads)
    {
        Dictionary<Vector3Int, Direction> freeSpots = new Dictionary<Vector3Int, Direction>();
        foreach (var pos in roads)
        {
            var neighbours = PlacementHelper.FindNeighbours(pos,roads);
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (!neighbours.Contains(dir))
                {
                    var newPos = pos + PlacementHelper.GetOffset(dir);
                    if (!freeSpots.ContainsKey(newPos))
                    {
                        freeSpots.Add(newPos, Direction.Right);
                    }

                }
            }
        }
        return freeSpots;
    }
}
