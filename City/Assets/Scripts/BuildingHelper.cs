using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class BuildingHelper : MonoBehaviour
{
    public int minimumMiddle = 1;
    public int maximumMiddle = 10;
    public BuildingType[] buildings;
    public Dictionary<Vector3Int, GameObject> dictionary = new Dictionary<Vector3Int, GameObject>();

    public void placeBuildings(List<Vector3Int> roads)
    {
        Dictionary<Vector3Int, Direction> freeSpots = FindFreeSpots(roads);
        foreach (var spot in freeSpots)
        {
            var rotation = Quaternion.identity;
            switch (spot.Value)
            {
                case Direction.Up:
                    rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.Down:
                    rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Direction.Right:
                    rotation = Quaternion.Euler(0, 180, 0);
                    break;
            }
            List<Vector3Int> visited = new List<Vector3Int>();
            var areaSize = FindAreaSize(spot.Key, roads,visited, 10,0);
            if (areaSize >= 8)
            {
                var building = SpawnSmallBuilding(spot.Key, rotation);
                dictionary.Add(spot.Key, building);
            }
            else 
            {
                var building = SpawnTallBuilding(spot.Key, rotation);
                dictionary.Add(spot.Key, building);
            }
        }
    }

    private int FindAreaSize(Vector3Int spot, List<Vector3Int> roads,List<Vector3Int> visited, int depth, int areaSize)
    {
        if (roads.Contains(spot)||visited.Contains(spot)||depth<=0)
        {
            if (depth<=0)
            {
                return 0;
            }
            return 0;
        }
        visited.Add(spot);
        areaSize = 1;
        areaSize += FindAreaSize(spot + Vector3Int.right,roads,visited,depth-1,areaSize);
        areaSize += FindAreaSize(spot - Vector3Int.right,roads,visited,depth-1,areaSize);
        areaSize += FindAreaSize(spot + new Vector3Int(0, 0, 1),roads,visited,depth-1,areaSize);
        areaSize += FindAreaSize(spot - new Vector3Int(0, 0, 1),roads,visited,depth-1,areaSize);
        return areaSize;
    }

    private GameObject SpawnSmallBuilding(Vector3Int pos, Quaternion rotation)
    {
        var newBuilding=Instantiate(buildings[0].getPrefab(), pos, rotation, transform);
        return newBuilding;
    }
    private GameObject SpawnTallBuilding(Vector3 pos, Quaternion rotation)
    {
        GameObject[] prefabs= buildings[1].getPrefabs();
        Mesh baseMesh = prefabs[0].GetComponentInChildren<MeshFilter>().sharedMesh;
        Mesh middleMesh = prefabs[1].GetComponentInChildren<MeshFilter>().sharedMesh;
        var baseObject = Instantiate(prefabs[0], pos, rotation, transform);

        float middleSizeSum = 0;
        var middleSize = middleMesh.bounds.size.y;
        var baseSize=baseMesh.bounds.size.y;
        for (int i=0;i<UnityEngine.Random.Range(minimumMiddle, maximumMiddle);i++)
        {
            Instantiate(prefabs[1], pos + new Vector3(0, baseSize+middleSizeSum, 0), rotation, transform);
            middleSizeSum+=middleSize;
        }

        Instantiate(prefabs[2], pos+new Vector3(0, baseSize+middleSizeSum,0), rotation, transform);
        return baseObject;
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
                        freeSpots.Add(newPos, PlacementHelper.GetDirection(dir));
                    }

                }
            }
        }
        return freeSpots;
    }
}
