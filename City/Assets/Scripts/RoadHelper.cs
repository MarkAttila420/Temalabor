using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
    public GameObject straight, corner, end, intersect3, intersect4;
    Dictionary<Vector3Int, GameObject> dictionary=new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadType=new HashSet<Vector3Int>();


    public void PlaceRoad(Vector3 start,Vector3Int dir, int length)
    {
        var rotation=Quaternion.identity;
        if (dir.x==0)
        {
            rotation = Quaternion.Euler(0,90,0);
        }
        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(start + dir * i);
            if (!dictionary.ContainsKey(position))
            {
                var road = Instantiate(straight, position, rotation);
                Debug.Log($"{position.x} {position.y} {position.z}");
                dictionary.Add(position, road);
                if (i == 0 || i == length - 1)
                {
                    fixRoadType.Add(position);
                }
            }
            
        }
    }

    public void FixRoad()
    {
        foreach (var position in fixRoadType)
        {
            List<Direction> neighbourDirections = PlacementHelper.FindNeighbours(position, dictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            if (neighbourDirections.Count == 1)
            {
                Destroy(dictionary[position]);
                if (neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                dictionary[position] = Instantiate(end, position, rotation, transform);
            }
            else if (neighbourDirections.Count == 2)
            {
                if (
                    neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Down)
                    || neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Left)
                    )
                {
                    continue;
                }
                Destroy(dictionary[position]);
                if (neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down) && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                dictionary[position] = Instantiate(corner, position, rotation, transform);
            }
            else if (neighbourDirections.Count == 3)
            {
                Destroy(dictionary[position]);
                if (neighbourDirections.Contains(Direction.Right)
                    && neighbourDirections.Contains(Direction.Down)
                    && neighbourDirections.Contains(Direction.Left)
                    )
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down)
                    && neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up)
                    && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                dictionary[position] = Instantiate(intersect3, position, rotation, transform);
            }
            else
            {
                Destroy(dictionary[position]);
                dictionary[position] = Instantiate(intersect4, position, rotation, transform);
            }
        }
    }
}
