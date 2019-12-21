using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ClipperLib;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ObstaclesMapGenerator : MonoBehaviour
{
    /// <summary>
    /// Ячейки, доля занятого пространства которых 
    /// больше этой величины, считаются занятыми
    /// </summary>
    public double PassabilityThreshold = 0.2;

    public int HalfWorldWidth = 20;

    public int HalfWorldHeight = 20;

    public Point WorldSize
    {
        get
        {
            return new Point(HalfWorldWidth*2, HalfWorldHeight*2);
        }
    }

    public Vector2 CellSize
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

    private void Start()
    {
        var mapGenerator = GameObject.Find("ObstaclesMapGenerator").GetComponent<ObstaclesMapGenerator>();
        PathFinder.GeneralMap = mapGenerator.GenerateMap();
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

        double[,] dResult = new double[WorldSize.X, WorldSize.Y];
        List<GameObject> allGOs = new List<GameObject>();
        SceneManager.GetActiveScene().GetRootGameObjects(allGOs);
        string allGOsStr = StringManipulation.ListToString(allGOs.Select(go => go.name).ToList());
        Debug.Log($"List of all GO's: {allGOsStr}");
        var allColliders = GetAllColliders(allGOs);
        for (int i = 0; i < allColliders.Count; i++)
        {
            var collider = allColliders[i];
            var go = allColliders[i].gameObject;
            var goPosition = new Vector2(go.transform.position.x, go.transform.position.y);
            var colliderPoints = GetPointsOfCollider(collider, goPosition);
            //-------------------------------------------------------------------------
            //Debug.Log($"There is a GO '{go.name}'.");
            //if (go.name.ToLower().Contains("campfire"))
            //{
            //    string colliderPointsStr = StringManipulation.ListToString(colliderPoints);
            //    Debug.Log($"GO '{go.name}' points: {colliderPointsStr}");
            //}
            //------------------------------------------------------------------------

            #region Comments
            //Теперь у нас есть координаты всех точек коллайдера
            //Поехали теперь находить площади пересечений этого коллайдера с ячейками карты.
            //Вопрос - каков по размеру мир игры? Думаю, пока хватит ромба 20х20 ячеек с центром в (0,0).
            #endregion

            for (int tx = -WorldSize.X / 2; tx < WorldSize.X / 2; tx++)
            {
                for (int ty = -WorldSize.Y / 2; ty < WorldSize.Y / 2; ty++)
                {
                    ProcessTileDuringMapGeneration(new Point(tx, ty), i, colliderPoints, dResult, go);
                }
            }
        }
        //---------------------------------------------------
        //Debug.Log($"Point (10, 8): {dResult[10, 8]}. Point (12,2): {dResult[12, 2]}");
        //---------------------------------------------------
        return GenerateIntArrayResult(dResult);
    }

    List<Collider2D> GetAllColliders(List<GameObject> allGOs)
    {
        var allColliders = new List<Collider2D>();
        foreach (GameObject go in allGOs)
            allColliders.AddRange(go.GetComponents<Collider2D>());
        return allColliders;
    }

    /// <summary>
    /// Возвращает долю занятой поверхности
    /// тайла с коорд. tileCoords коллайдером
    /// с точками colliderPoints.
    /// Помимо этого коллайдера, учитывает также
    /// коллайдер тайла-препятствия, если таковой на этом тайле присутствует
    /// </summary>
    /// <param name="tileCoords"></param>
    /// <param name="currentColliderIndex"></param>
    /// <param name="colliderPoints"></param>
    /// <param name="resultArray"></param>
    /// <param name="currentColliderGameObject"></param>
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
        var coordSystemConverter = new TileCoordSystemConverter(WorldSize, CellSize);
        Point usCoords = new Point(tx, ty);
        Point pfsCoords = coordSystemConverter.US_To_PFS(usCoords);
        //Сразу же проверим, является ли этот тайл тайлом-препятствием
        //И если да, то возвращаем 1 - полностью занятый тайл
        if (TileIsObstacle(usCoords))
        {
            resultArray[pfsCoords.X, pfsCoords.Y] = 1f;
            return;
        }
        var cell = GetVerticesOfCell(usCoords);
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
        if (intersectionInfo.CellFraction != 0 
         && !currentColliderGameObject.name.ToLower().Contains("letov"))
        {
            Debug.Log(intersectionInfo);
            Debug.Log("Cell (World):" + StringManipulation.ListToString(cell));
            Debug.Log($"Cell (US): {tileCoords}");
            Debug.Log("Collider points (World): " + StringManipulation.ListToString<Vector2>(colliderPoints));
        }
        //-------------------------------------------------------------     
        resultArray[pfsCoords.X, pfsCoords.Y] += occupiedFraction;
    }

    bool TileIsObstacle(Point tileUS_Coords)
    {
        var tilemap = GetTilemap("ObstacleTiles TEST");
        var tile = tilemap.GetTile(new Vector3Int(tileUS_Coords.X-1, tileUS_Coords.Y-1, 0));
        return TileIsObstacle(tile);
    }

    bool TileIsObstacle(TileBase tile)
    {
        bool res = tile != null;
        return res;
    }

    Tilemap GetTilemap(string name)
    {
        return FindObjectsOfType<Tilemap>().FirstOrDefault(tm => tm.name == name);
    }

    List<Vector2> GetPointsOfCollider(Collider2D collider, Vector2 objPosition)
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
            var pointsArray = polygonCollider.points;

            #region Comments
            //Получили массив точек коллайдера в системе координат, привязанной к левому верхнему углу объекта.
            //Теперь получим настоящие координаты точек коллайдера, 
            //сложив полученные точки с левой верхней точкой объекта.
            #endregion

            for (int i = 0; i < pointsArray.Length; i++)
            {
                //Debug.Log($"Adding {objPosition} to {pointsArray[i]}");
                pointsArray[i] = polygonCollider
                    .transform
                    .TransformPoint(pointsArray[i].x, pointsArray[i].y, 0f);
            }

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
        int[,] result = new int[WorldSize.X, WorldSize.Y];
        for(int tx = 0; tx < WorldSize.X; tx++)
            for (int ty = 0; ty < WorldSize.X; ty++)
            {
                double occupiedFraction = arrayOfOccupiedFractions[tx, ty];
                int cellStatus = EMPTY;
                if (occupiedFraction > PassabilityThreshold)
                {
                    //Отметим ячейку как занятую
                    cellStatus = OCCUPIED;
                }
                result[tx, ty] = cellStatus;
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
               ,C00 = new Vector2(0, -csy)
               ,D00 = new Vector2(-csx / 2, -csy / 2);
        Vector2 A = new Vector2((tx - ty) * csx / 2, (tx + ty) * csy / 2)
               ,B = A+B00
               ,C = A+C00
               ,D = A+D00;
        return new List<Vector2> { A, B, C, D };
    }

    Vector2 dummyPoint = new Vector2(int.MinValue, int.MinValue);

    public static int EMPTY = 0;

    public static int OCCUPIED = 1;
}
