using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileCoordSystemConverter
{
    public TileCoordSystemConverter(Point worldSize, Vector2 cellSize)
    {
        WorldSize = worldSize;
        CellSize = cellSize;
    }

    Point WorldSize { get; set; }

    Vector2 CellSize { get; set; }

    public Point PFS_To_US(Point pfsCoords)
    {
        return new Point(pfsCoords.X - WorldSize.X / 2, pfsCoords.Y - WorldSize.Y / 2);
    }

    public Point US_To_PFS(Point usCoords)
    {
        return new Point(usCoords.X + WorldSize.X / 2, usCoords.Y + WorldSize.Y / 2);
    }

    public Point UW_To_PFS(Vector2 uwCoords)
    {
        return US_To_PFS(UW_To_US(uwCoords));
    }

    public Point UW_To_US(Vector2 uwCoords)
    {
        Vector2 vector = new Vector2(uwCoords.x / CellSize.x + uwCoords.y / CellSize.y
                                    , uwCoords.y / CellSize.y - uwCoords.x/ CellSize.x );
        Point result = new Point((int)(Math.Round(vector.x)), (int)(Math.Round(vector.y)));
        return result;
    }

    public Vector2 US_To_UW(Point usCoords, PositionInCell pointInCell = PositionInCell.CENTER)
    {
        var topCorner = new Vector2((usCoords.X - usCoords.Y) * CellSize.x / 2, (usCoords.X + usCoords.Y) * CellSize.y / 2);
        Vector2 bias = new Vector2();
        if (pointInCell == PositionInCell.CENTER)
            bias = new Vector2(0f, -CellSize.y / 2f);
        if (pointInCell == PositionInCell.DOWNCORNER)
            bias = new Vector2(0f, -CellSize.y);
        if (pointInCell == PositionInCell.LEFTCORNER)
            bias = new Vector2(-CellSize.x / 2f, -CellSize.y / 2f);
        if (pointInCell == PositionInCell.RIGHTCORNER)
            bias = new Vector2(CellSize.x / 2f, -CellSize.y / 2f);
        if (pointInCell == PositionInCell.TOPCORNER)
            bias = new Vector2(0f, 0f);
        Vector2 result = topCorner + bias;
        return result;
    }
}

public enum PositionInCell { CENTER, TOPCORNER, DOWNCORNER, LEFTCORNER, RIGHTCORNER }
