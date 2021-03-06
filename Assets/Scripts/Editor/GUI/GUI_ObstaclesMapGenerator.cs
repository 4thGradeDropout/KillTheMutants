﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstaclesMapGenerator))]
public class GUI_EasyImport_Decorations : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ObstaclesMapGenerator script = (ObstaclesMapGenerator)target;
        if (GUILayout.Button("Сгенерировать общую карту"))
        {
            PathFinder.GeneralMap = script.GenerateMap();
            //string arrayStr = IntArrayToString(obstaclesMap);
            //var occupiedTiles = GetOccupiedTilesCoords(PathFinder.GeneralMap);
            //----------------
            //Debug.Log("Occupied tiles below:");
            //var converter = new TileCoordSystemConverter(script.WorldSize);
            //occupiedTiles.ForEach(tileCoords => Debug.Log(converter.PFS_To_US(tileCoords)));
            //----------------
        }
    }

    public List<Point> GetOccupiedTilesCoords(int[,] map)
    {
        List<Point> res = new List<Point>();
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x,y] == ObstaclesMapGenerator.OCCUPIED)
                    res.Add(new Point(x, y));
            }
        }
        return res;
    }

    public string IntArrayToString(int[,] arr)
    {
        string res = "";
        for (int x = 0; x < arr.GetLength(0); x++)
        {
            for (int y = 0; y < arr.GetLength(1); y++)
            {
                res += $"{arr[x, y]}, ";
            }
            res += "\n";
        }
        return res;
    }

    
}
