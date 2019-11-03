using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClipperLib;
using System;

public class ClipperForFloats
{
    public ClipperForFloats()
    {
        TheClipper = new Clipper();
    }

    public Clipper TheClipper { get; set; }

    //Требуется, чтобы перевести флоаты в инты, 
    //так как клиппер ест инты
    int ClipperUnitsPerUnityUnit = 1000;
    
    public void AddPolygon(List<Vector2> polygon, PolyType type)
    {
        List<IntPoint> preparedPolygon = new List<IntPoint>();
        foreach (Vector2 origVertice in polygon)
        {
            Vector2 multipliedVector = ClipperUnitsPerUnityUnit * origVertice;
            Vector2 rounded = new Vector2((float)Math.Round(multipliedVector.x), (float)Math.Round(multipliedVector.y));
            IntPoint preparedVertice = new IntPoint((int)rounded.x, (int)rounded.y);
            preparedPolygon.Add(preparedVertice);
        }
        TheClipper.AddPolygon(preparedPolygon, type);
    }

    public double GetIntersectionArea()
    {
        var resultPolygons = new List<List<IntPoint>>();
        TheClipper.Execute(ClipType.ctIntersection, resultPolygons, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
        double area = 0;
        resultPolygons.ForEach(poly => area += Clipper.Area(poly));
        //Теперь у нас есть площадь, рассчитанная для сетки, увеличенной в CUPUU в квадрате раз
        //Разделим ее на это число - получим настоящую площадь
        double originalArea = area / (ClipperUnitsPerUnityUnit * ClipperUnitsPerUnityUnit);
        return originalArea;
    }
}
