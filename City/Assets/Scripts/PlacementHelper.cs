using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class PlacementHelper
{
    //Megadja, hogy a 'pos'-hoz kepest melyik iranyban talalhatoak 'collection' beli elemek 
    public static List<Direction> FindNeighbours(Vector3Int pos, ICollection<Vector3Int> collection)
    {
        List<Direction> neighbourDirections = new List<Direction>();
        if (collection.Contains(pos + Vector3Int.right)) neighbourDirections.Add(Direction.Right);
        if (collection.Contains(pos - Vector3Int.right)) neighbourDirections.Add(Direction.Left);
        if (collection.Contains(pos + new Vector3Int(0, 0, 1))) neighbourDirections.Add(Direction.Up);
        if (collection.Contains(pos - new Vector3Int(0, 0, 1))) neighbourDirections.Add(Direction.Down);
        return neighbourDirections;
    }

    //Az iranynat az ellenkezojet adja vissza
    public static Direction GetDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            default:
                return Direction.Left;
        }
    }

    //Ez a fuggveny az epulet elhelyezesenel van hasznalva, a 'dir' az azt jelenti, hogy az uttol melyik iranyba van a lehelyezendo ut
    internal static Vector3Int GetOffset(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector3Int(0, 0, 1);
            case Direction.Down:
                return new Vector3Int(0, 0, -1);
            case Direction.Left:
                return Vector3Int.left;
            default:
                return Vector3Int.right;
        }
    }
}
