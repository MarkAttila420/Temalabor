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
        if (dir.x!=0)
        {
            rotation = Quaternion.Euler(0,90,0);
        }
        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(start+dir*i);
            if (!dictionary.ContainsKey(position))
            {
                var road = Instantiate(straight, position, rotation, transform);
                dictionary.Add(position, road);
                if (i==0||i==length-1)
                {
                    fixRoadType.Add(position);
                }
            }
        }
    }
}
