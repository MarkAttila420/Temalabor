using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHelper : MonoBehaviour
{
    private int minimumMiddle = 1;
    private int maximumMiddle = 100;
    [SerializeField] private Slider maxSlider;
    [SerializeField] private TextMeshProUGUI maxText;
    [SerializeField] private Slider minSlider;
    [SerializeField] private TextMeshProUGUI minText;
    public BuildingType[] buildings;
    public Dictionary<Vector3Int, GameObject> dictionary = new Dictionary<Vector3Int, GameObject>();


    private void Start()
    {
        maxSlider.value = maximumMiddle;
        minSlider.value = minimumMiddle;
        maxText.text = new StringBuilder($"Maximum building height: {maximumMiddle.ToString()}").ToString();
        minText.text = new StringBuilder($"Minimum building height: {minimumMiddle.ToString()}").ToString();
        minSlider.onValueChanged.AddListener((value) =>
        {
            if (value > maximumMiddle)
            {
                minimumMiddle = maximumMiddle;
                maximumMiddle = (int)Math.Round(value);
                maxSlider.value = minimumMiddle;
            }
            else
            {
                minimumMiddle = (int)Math.Round(value);
            }
            minText.text = new StringBuilder($"Minimum building height: {minimumMiddle.ToString()}").ToString();
        });
        maxSlider.onValueChanged.AddListener((value) =>
        {
            if (value < minimumMiddle)
            {
                maximumMiddle = minimumMiddle;
                minimumMiddle = (int)Math.Round(value);
                minSlider.value = maximumMiddle;
            }
            else
            {
                maximumMiddle = (int)Math.Round(value);
            }
            maxText.text = new StringBuilder($"Maximum building height: {maximumMiddle.ToString()}").ToString();
        });
    }
    
    public List<Vector3Int> GetBuildings() 
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        foreach (var item in dictionary.Keys)
        {
            if (item.y==0)
            {
                positions.Add(item); 
            }
        }
        return positions;

    }

    //Ez a fuggveny helyezi le az osszes epuletet, az utak korul.
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
                int i = 0;
                foreach (var item in building)
                {
                    dictionary.Add(new Vector3Int(spot.Key.x,i,spot.Key.z),item);
                    i++;
                }
            }
        }
    }

    //DFS: megnezi, hogy az adott epulet koordinataja korul mennyi szabad hely van, es ha kevesebb, mint 8, akkor oda egy magas epuletet rak le, ellenkezo esetben pedig egy kerteshazat. 
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

    //Ez a fuggveny megjelenit egy kerteshazat.
    private GameObject SpawnSmallBuilding(Vector3Int pos, Quaternion rotation)
    {
        var newBuilding=Instantiate(buildings[0].getPrefab(), pos, rotation, transform);
        return newBuilding;
    }

    //Ez a fuggveny megjelenit egy felhokarcolot, ugy hogy megjeleniti a legaljat eloszor, es utana veletlenszeruen valamennyi kozepso reszt, majd egy tetot.
    private List<GameObject> SpawnTallBuilding(Vector3 pos, Quaternion rotation)
    {
        List<GameObject> tallBuilding = new List<GameObject>();
        if (buildings.Length >= 2)
        {
            int buildingsIndex = UnityEngine.Random.Range(1, buildings.Length);
            GameObject[] prefabs = buildings[buildingsIndex].getPrefabs();
            Mesh baseMesh = prefabs[0].GetComponentInChildren<MeshFilter>().sharedMesh;
            Mesh middleMesh = prefabs[1].GetComponentInChildren<MeshFilter>().sharedMesh;
            var baseObject = Instantiate(prefabs[0], pos, rotation, transform);
            tallBuilding.Add(baseObject);

            float middleSizeSum = 0;
            var middleSize = middleMesh.bounds.size.y;
            var baseSize = baseMesh.bounds.size.y;
            for (int i = 0; i < UnityEngine.Random.Range(minimumMiddle, maximumMiddle); i++)
            {
                var tempMiddleObject=Instantiate(prefabs[1], pos + new Vector3(0, baseSize + middleSizeSum, 0), rotation, transform);
                tallBuilding.Add(tempMiddleObject);
                middleSizeSum += middleSize;
            }

            var topObject = Instantiate(prefabs[2], pos + new Vector3(0, baseSize + middleSizeSum, 0), rotation, transform);
            tallBuilding.Add(topObject);
        }
        else
        {
            var newBuilding = Instantiate(buildings[0].getPrefab(), pos, rotation, transform);
            tallBuilding.Add(newBuilding);
        }
        return tallBuilding;
    }

    //Ez a fuggveny visszaadja, hogy az utak kore hova lehet elhelyezni epuletet
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

    public void delete() 
    {
        foreach (var item in dictionary.Values)
        {
            Destroy(item);
        }
        dictionary.Clear();
    }
}
