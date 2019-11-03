using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAndCellIntersectionInfo
{
    public ColliderAndCellIntersectionInfo()
    {

    }

    public ColliderAndCellIntersectionInfo(double cf, Point cc, GameObject go, int caei)
    {
        CellFraction = cf;
        CellCoords = cc;
        TheGameObject = go;
        ColliderArrayElementIndex = caei;
    }

    /// <summary>
    /// Доля площади ячейки, покрытая этим коллайдером
    /// </summary>
    public double CellFraction { get; set; }

    public Point CellCoords { get; set; }

    public GameObject TheGameObject { get; set; }

    public int ColliderArrayElementIndex { get; set; }

    public override string ToString()
    {
        return $"Si={CellFraction} at " +
               $"({CellCoords.X},{CellCoords.Y}) " +
               $"on object {TheGameObject} number " +
               $"#{ColliderArrayElementIndex}";
    }
}
