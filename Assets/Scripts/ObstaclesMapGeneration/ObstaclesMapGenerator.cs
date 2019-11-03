using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ClipperLib;

public class ObstaclesMapGenerator : MonoBehaviour
{
    Vector2 CellSize
    {
        get
        {
            var allGrids = GameObject.FindObjectsOfType<Grid>();
            var firstGrid = allGrids.FirstOrDefault();
            if (firstGrid == null)
                return dummyPoint;
            return new Vector2(firstGrid.cellSize.x, firstGrid.cellSize.y);
        }
    }

    double CellArea
    {
        get
        {
            return CellSize.x * CellSize.y / 2;
        }
    }

    public int[,] GenerateMap()
    {
        #region Comments
        //0 у нас - свободно, а 1 - занято.
        //Придется ввести две системы изометрических координат - 
        //одна с центром в левом верхнем краю мира (для алгоритма поиска пути) PFS,
        //другая с центром в нуле координат Unity (для Unity) US
        //координаты точки (0,0) в PFS - (WSx/2, WSy/2)
        //Решение для каждой точки будет найдено сначала в US, затем переведено в PFS
        //Сперва найдем решение как массив double, в котором будет инфа о доли занятой площади каждой ячейки
        //Затем переведем этот массив double в массив int, который уже - окончательный результат
        //Оба массива будут в PFS
        #endregion

        double[,] dResult = new double[worldSize.X, worldSize.Y];
        var allGOs = GameObject.FindObjectsOfType<MonoBehaviour>();
        var allColliders = new List<Collider2D>();
        foreach (MonoBehaviour go in allGOs)
            allColliders.AddRange(go.GetComponents<Collider2D>());
        for (int i = 0; i < allColliders.Count; i++)
        {
            var collider = allColliders[i];
            var go = allColliders[i].gameObject;
            var colliderPoints = GetPointsOfCollider(collider);
            //Теперь у нас есть координаты всех точек коллайдера
            //Поехали теперь находить площади пересечений этого коллайдера с ячейками карты.
            //Вопрос - каков по размеру мир игры? Думаю, пока хватит ромба 20х20 ячеек вокруг ячейки (0,0).
            for (int tx = -worldSize.X / 2; tx < worldSize.X / 2; tx++)
            {
                for (int ty = -worldSize.Y / 2; ty < worldSize.Y / 2; ty++)
                {
                    ProcessTileDuringMapGeneration(new Point(tx, ty), i, colliderPoints, dResult, go);
                }
            }
        }
        //---------------------------------------------------
        Debug.Log($"Point (10, 8): {dResult[10, 8]}. Point (12,2): {dResult[12, 2]}");
        //---------------------------------------------------
        return GenerateIntArrayResult(dResult);
    }

    void ProcessTileDuringMapGeneration
        (
            Point tileCoords
           ,int currentColliderIndex
           ,List<Vector2> colliderPoints
           ,double[,] resultArray
           ,GameObject currentColliderGameObject
        )
    {
        int tx = tileCoords.X, ty = tileCoords.Y, i = currentColliderIndex;
        var cell = GetVerticesOfCell(new Point(tx, ty));
        ClipperForFloats clipper = new ClipperForFloats();
        clipper.AddPolygon(cell, PolyType.ptSubject);
        clipper.AddPolygon(colliderPoints, PolyType.ptClip);
        double intersectionArea = clipper.GetIntersectionArea();
        double occupiedFraction = intersectionArea / CellArea;
        var intersectionInfo = new ColliderAndCellIntersectionInfo
            (
                occupiedFraction,
                tileCoords,
                currentColliderGameObject,
                i
            );
        //-------------------------------------------------------------
        if (intersectionInfo.CellFraction != 0)
            Debug.Log(intersectionInfo);
        //-------------------------------------------------------------
        //переведем в PFS и запишем в result.
        var coordSystemConverter = new TileCoordSystemConverter(worldSize);
        Point usCoords = new Point(tx, ty);
        Point pfsCoords = coordSystemConverter.US_To_PFS(usCoords);
        resultArray[pfsCoords.X, pfsCoords.Y] += occupiedFraction;
    }

    List<Vector2> GetPointsOfCollider(Collider2D collider)
    {
        List<Vector2> colliderPoints = new List<Vector2>();
        if (collider is BoxCollider2D)
        {
            var boxCollider = collider as BoxCollider2D;
            var bounds = boxCollider.bounds;
            colliderPoints = GetVerticesOfRectBounds(bounds);
        }
        else if (collider is PolygonCollider2D)
        {
            var polygonCollider = collider as PolygonCollider2D;
            var pointsArray = polygonCollider.GetPath(0);
            colliderPoints = pointsArray.ToList();
        }
        return colliderPoints;
    }

    List<double> ArrayToList(double[,] ar)
    {
        List<double> result = new List<double>();
        foreach (double num in ar)
            result.Add(num);
        return result;
    }

    int[,] GenerateIntArrayResult(double[,] arrayOfOccupiedFractions)
    {
        int[,] result = new int[worldSize.X, worldSize.Y];
        for(int tx = 0; tx < worldSize.X; tx++)
            for (int ty = 0; ty < worldSize.X; ty++)
            {
                double occupiedFraction = arrayOfOccupiedFractions[tx, ty];
                int occupied = 0;
                if (occupiedFraction > passabilityThreshold)
                {
                    //Отметим ячейку как занятую
                    occupied = 1;
                }
                result[tx, ty] = occupied;
            }
        return result;
    }

    List<Vector2> GetVerticesOfRectBounds(Bounds bounds)
    {
        return new List<Vector2>()
                {
                     bounds.min
                    ,bounds.min+new Vector3(bounds.size.x, 0)
                    ,bounds.min+new Vector3(0, bounds.size.y)
                    ,bounds.max
                };
    }

    List<Vector2> GetVerticesOfCell(Point thePoint)
    {
        int tx = thePoint.X, ty = thePoint.Y;
        var cellSize = CellSize;
        float csx = cellSize.x, csy = cellSize.y;
        var result = new List<Vector2>();
        Vector2 A00 = new Vector2(0, 0)
               ,B00 = new Vector2(csx / 2, -csy / 2)
               ,C00 = new Vector2(0, csy)
               ,D00 = new Vector2(-csx / 2, -csy / 2);
        Vector2 A = new Vector2(tx * csx, ty * csy)
               ,B = A+B00
               ,C = A+C00
               ,D = A+D00;
        return new List<Vector2> { A, B, C, D };
    }

    public static Point worldSize = new Point(20, 20);

    Vector2 dummyPoint = new Vector2(int.MinValue, int.MinValue);

    double passabilityThreshold = 0.5;
}
