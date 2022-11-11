using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class PlacementHelper
{
    public static List<Direction> FindNeighbours(Vector3Int pos, ICollection<Vector3Int> collection)
    {
        List<Direction> neighbourDirections = new List<Direction>();
        if (collection.Contains(pos + Vector3Int.right)) neighbourDirections.Add(Direction.Right);
        if (collection.Contains(pos - Vector3Int.right)) neighbourDirections.Add(Direction.Left);
        if (collection.Contains(pos + new Vector3Int(0, 0, 1))) neighbourDirections.Add(Direction.Up);
        if (collection.Contains(pos - new Vector3Int(0, 0, 1))) neighbourDirections.Add(Direction.Down);
        return neighbourDirections;
    }
}
