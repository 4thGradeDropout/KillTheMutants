using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Cell
{
    public Vector2 BottomPoint { get; set; }

    public Vector2 LeftPoint { get; set; }

    public Point IsometricCoords { get; set; }

    public override string ToString()
    {
        return $"Iso-coords: {IsometricCoords}. Bottom: {BottomPoint}; Left: {LeftPoint}";
    }

    public bool PointIsIn(Vector2 p)
    {
        //Формулы переделать с учетом сдвига по Х
        return p.y <= p.x / 2 + CellSize.y * IsometricCoords.Y
            && p.y <= -p.x / 2 + CellSize.y * IsometricCoords.Y
            && p.y >= p.x / 2 + CellSize.y * IsometricCoords.Y - CellSize.y
            && p.y >= -p.x / 2 + CellSize.y * IsometricCoords.Y - CellSize.y;
        
    }

    public static void SetGrid(Grid g)
    {
        SceneGrid = g;
    }

    public static Grid SceneGrid;

    public static Vector2 CellSize
    {
        get
        {
            return new Vector2(SceneGrid.cellSize.x, SceneGrid.cellSize.y);
        }
    }
}
