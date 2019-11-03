using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCoordSystemConverter
{
    public TileCoordSystemConverter(Point worldSize)
    {
        WorldSize = worldSize;
    }

    Point WorldSize { get; set; }

    public Point PFS_To_US(Point pfsCoords)
    {
        return new Point(pfsCoords.X - WorldSize.X / 2, pfsCoords.Y - WorldSize.Y / 2);
    }

    public Point US_To_PFS(Point usCoords)
    {
        return new Point(usCoords.X + WorldSize.X / 2, usCoords.Y + WorldSize.Y / 2);
    }
}
