using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Ez az osztaly jeleniti meg a varost.
public class Visualizer : MonoBehaviour
{
    public LSystem lsystem;

    public RoadHelper roadHelper;
    public BuildingHelper buildingHelper;
    public GroundHelper groundHelper;
    public PedestrianSpawner pedestrian;
    public NavMeshBaker navMeshBaker;

    private int length=2;

    public int Length
    {
        get => length;
        set => length = value;
    }

    private void Start()
    {
        create();
    }

    //ez a fuggveny hivodik meg minding, amikor  felhasznalo megnyomja a generate gombot, es a jatek indulasakor, eloszor torli az elozoleg legeneralt varost, es ujat hoz letre.
    public void create()
    {
        length = 2;
        roadHelper.delete();
        buildingHelper.delete();
        groundHelper.delete();
        pedestrian.delete();
        var sequence = lsystem.Generate();
        VisualizeSequence(sequence);
        navMeshBaker.bake();
        roadHelper.attachObstacles();
        pedestrian.spawnAllPerson(lsystem.numberOfPedestrians);
    }

    //Az Lsystem altal legeneralt stringet megjeleniti.
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
                case EncodingLetters.save://elmenti azt a poziciot, ahol jelenleg van a "teknos".
                    savePoints.Push(new AgentParameters
                    {
                        pos = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load://visszamegy arra a poziciora, amit legutoljara mentett el.
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
                    //Azert van draw es draw2, mert az F es a G betu hatasara, ugyanugy rajzolnia kell a "teknosnek", de az F-hez es a G-hez mas szabajok tartoznak.
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
                    direction = Quaternion.AngleAxis(90, Vector3.up) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-90, Vector3.up) * direction;
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
