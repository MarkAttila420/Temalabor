using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Ez az osztaly felelos az utak elhelyezesert
public class RoadHelper : MonoBehaviour
{
    public GameObject straight, corner, end, intersect3, intersect4;

    Dictionary<Vector3Int, GameObject> dictionary=new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadType=new HashSet<Vector3Int>();

    public List<Vector3Int> getRoads()
    {
        return dictionary.Keys.ToList();
    }

    //Lerak 'length' darab utat a 'start' koordinatabol kiindul, a 'dir' iranyba.
    //Ezek kozul az elsot, meg az utolsot amit lerakott belerakja a megfixalando utak-ba, mivel ez a ketto vagy egy ut veget jelentik, vagy egy kanyart, vagy egy keresztezodest. 
    public void PlaceRoad(Vector3 start,Vector3Int dir, int length)
    {
        var rotation=Quaternion.identity;
        if (dir.x==0)
        {
            rotation = Quaternion.Euler(0,90,0);
        }
        for (int i = 0; i < length; i++)
        {
            var pos = Vector3Int.RoundToInt(start + dir * i);
            if (!dictionary.ContainsKey(pos))
            {
                var road = Instantiate(straight, pos, rotation);
                dictionary.Add(pos, road);
                if (i == 0 || i == length - 1)
                {
                    fixRoadType.Add(pos);
                }
            }
            
        }
    }

    //Azokat az utakat, amiket beleraktunk a megfixalandokba, azokon vegigmegy, es az alapjan, hogy hany szomszedja van, es azok milyen iranyba neznek, kicsereli es elforgatja oket.
    public void FixRoad()
    {
        foreach (var pos in fixRoadType)
        {
            List<Direction> neighbours = PlacementHelper.FindNeighbours(pos, dictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            if (neighbours.Count == 1)
            {
                Destroy(dictionary[pos]);
                if (neighbours.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbours.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbours.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                dictionary[pos] = Instantiate(end, pos, rotation, transform);
            }
            else if (neighbours.Count == 2)
            {
                if (neighbours.Contains(Direction.Up) && neighbours.Contains(Direction.Down)|| neighbours.Contains(Direction.Right) && neighbours.Contains(Direction.Left))
                {
                    continue;
                }
                Destroy(dictionary[pos]);
                if (neighbours.Contains(Direction.Up) && neighbours.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbours.Contains(Direction.Right) && neighbours.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbours.Contains(Direction.Down) && neighbours.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                dictionary[pos] = Instantiate(corner, pos, rotation, transform);
            }
            else if (neighbours.Count == 3)
            {
                Destroy(dictionary[pos]);
                if (!neighbours.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (!neighbours.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (!neighbours.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                dictionary[pos] = Instantiate(intersect3, pos, rotation, transform);
            }
            else
            {
                Destroy(dictionary[pos]);
                dictionary[pos] = Instantiate(intersect4, pos, rotation, transform);
            }
        }
    }

    public void delete()
    {
        foreach (var item in dictionary.Values)
        {
            Destroy(item);
        }
        dictionary.Clear();
        fixRoadType = new HashSet<Vector3Int>();
    }
}
