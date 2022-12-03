using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ez az osztaly felelos a fold-ek lerakasaert
public class GroundHelper : MonoBehaviour
{
    public GameObject[] groundTypes;
    public Dictionary<Vector3Int, GameObject> dictionary = new Dictionary<Vector3Int, GameObject>();
    public int sizeAroundBuildings = 5;


    //minden epulet korul lesz 'sizeAroundBuildings'*2+1 teruletu fold.
    public void placeGrounds(List<Vector3Int> buildings, List<Vector3Int> roads)
    {
        foreach (var item in buildings)
        {
            placeGroundAroundBuilding(item, buildings, roads);
        }
    }
    //Az adott epulet koruli 'sizeAroundBuildings'*2+1 teruleten az osszes olyan koordinatara, ami nem epulet vagy ut foldet tesz.
    public void placeGroundAroundBuilding(Vector3Int pos, List<Vector3Int> buildings, List<Vector3Int> roads)
    {
        for (int i = -sizeAroundBuildings; i <= sizeAroundBuildings; i++)
        {
            for (int j = -sizeAroundBuildings; j <= sizeAroundBuildings; j++)
            {
                Vector3Int tempPos = pos + new Vector3Int(i,0,j);
                if (!buildings.Contains(tempPos)&&!roads.Contains(tempPos)&&!dictionary.ContainsKey(tempPos))
                {
                    var newGround = Instantiate(groundTypes[UnityEngine.Random.Range(0,groundTypes.Length)],tempPos,Quaternion.Euler(0,UnityEngine.Random.Range(0,4)*90,0),transform);
                    dictionary.Add(tempPos,newGround);
                }
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
    }
}
