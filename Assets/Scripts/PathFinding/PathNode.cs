using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Point Position { get; set; }
    // Длина пути от старта (G).
    public int PathLengthFromStart { get; set; }
    // Точка, из которой пришли в эту точку.
    public PathNode CameFrom { get; set; }
    // Примерное расстояние до цели (H).
    public int HeuristicEstimatePathLength { get; set; }
    // Ожидаемое полное расстояние до цели (F).
    public int EstimateFullPathLength
    {
        get
        {
            return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
        }
    }

    public override string ToString()
    {
        string res = Position.ToString() + ". d(S) = ";
        res += PathLengthFromStart.ToString();
        if (CameFrom != null)
            res += ". prev = " + CameFrom.Position.ToString();
        res += ". h(F) = " + HeuristicEstimatePathLength.ToString() + ".";
        return res;
    }
}
