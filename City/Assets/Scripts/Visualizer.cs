using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SimpleVisualizer;

public class Visualizer : MonoBehaviour
{
    public LSystem lsystem;

    public RoadHelper roadHelper;
    public BuildingHelper buildingHelper;
    public GroundHelper groundHelper;

    private int length=2;
    private float angle = 90;

    public int Length
    {
        get
        {
            if (length > 1)
            {
                return length;
            }
            else
            {
                return UnityEngine.Random.Range(2,4);
            }
        }
        set => length = value;
    }

    private void Start()
    {
        create();
    }

    public void create()
    {
        length = 2;
        roadHelper.delete();
        buildingHelper.delete();
        groundHelper.delete();
        var sequence = lsystem.Generate();
        VisualizeSequence(sequence);
    }

    private void VisualizeSequence(string sequence)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
        var currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;


        foreach (var letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameters
                    {
                        pos = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.pos;
                        direction = agentParameter.direction;
                        Length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("Dont have saved point in our stack");
                    }
                    break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * Length;
                    roadHelper.PlaceRoad(tempPosition,Vector3Int.RoundToInt(direction),Length);
                    break;
                case EncodingLetters.draw2:
                    tempPosition = currentPosition;
                    currentPosition += direction * Length;
                    roadHelper.PlaceRoad(tempPosition, Vector3Int.RoundToInt(direction), Length);
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
                default:
                    break;
            }
        }
        roadHelper.FixRoad();
        buildingHelper.placeBuildings(roadHelper.getRoads());
        groundHelper.placeGrounds(buildingHelper.GetBuildings(),roadHelper.getRoads());
    }
}
