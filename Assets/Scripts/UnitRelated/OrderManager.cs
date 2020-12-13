using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderManager : MonoBehaviour
{
    protected OrderMark orderMark;
    protected SelectionManager selectionManager;

    float maxAverageScatterDistance = 1.2f;

    TileCoordSystemConverter CoordConverter { get; set; }

    List<UnitSelection> SelectedUnits
    {
        get
        {
            return selectionManager.SelectedUnits;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        var orderMarkGO = GameObject.Find("OrderMark");
        orderMark = GetComponentInChildren<OrderMark>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    

    public void MakeRightClickOrder(Vector3 clickedPosition)
    {
        
        //Debug.Log("Order created: move to " + clickedPosition.ToString());
        Vector3 PositionForMark = new Vector3(clickedPosition.x, clickedPosition.y, orderMark.ZCoordinate);
        orderMark.InstantlyMoveTo(PositionForMark);
        orderMark.gameObject.SetActive(true);

        //We should give different units orders to move to different locations,
        //otherwise they will often collide.
        Vector2 where = new Vector2(clickedPosition.x, clickedPosition.y);
        Vector2[] FinPoints = ScatterFinishPointsOfOrderForSelectedUnits(where);
        for (int i = 0; i < SelectedUnits.Count; i++)
        {
            var navigator = SelectedUnits[i].navigator;
            navigator.FinishPoint = FinPoints[i];
            if (!InstructNavigator(navigator))
            {
                continue;
            }
        }
    }

    /// <summary>
    /// Генерирует набор команд для попадания в finPoint и отдаёт этот набор навигатору юнита.
    /// </summary>
    /// <param name="finPoint"></param>
    /// <returns></returns>
    public bool InstructNavigator(Navigator unitNavigator)
    {
        var pfsPath = GetPfsPath(unitNavigator.UnitCoords, unitNavigator.FinishPoint);
        if (pfsPath == null)
        {
            return false;
        }
        //Получили путь в PFS. Теперь получим путь в US.
        var uwPath = GetUwPath(pfsPath);
        CreateAndSendNavigatorCommands(uwPath, unitNavigator);
        return true;
    }

    void CreateAndSendNavigatorCommands(List<Vector2> uwPath, Navigator unitNavigator)
    {
        PathToCommandConverter pathToCmdConverter = new PathToCommandConverter();
        var commands = pathToCmdConverter.PathToCommands(uwPath, unitNavigator);
        unitNavigator.ReceiveCommands(commands);
    }

    List<Vector2> GetUwPath(List<Point> pfsPath)
    {
        var usPath = pfsPath.Select(p => CoordConverter.PFS_To_US(p)).ToList();
        //---------------------------------------------------------------------------------------
        //Debug.Log("НАЙДЕННЫЙ ПУТЬ: " + StringManipulation.ListToString(usPath));
        //---------------------------------------------------------------------------------------
        //А теперь путь в UW:
        var uwPath = usPath.Select(p => CoordConverter.US_To_UW(p)).ToList();
        uwPath.Remove(uwPath[0]);
        return uwPath;
    }

    List<Point> GetPfsPath(Vector2 unitCoords, Vector2 finPoint)
    {
        //Now find path to that point:
        var pathFinder = new PathFinder();
        var mapGenerator = GameObject.Find("ObstaclesMapGenerator").GetComponent<ObstaclesMapGenerator>();
        CoordConverter = new TileCoordSystemConverter(mapGenerator.WorldSize, mapGenerator.CellSize);
        Point start = CoordConverter.UW_To_PFS(unitCoords);
        Point goal = CoordConverter.UW_To_PFS(finPoint);
        var pfsPath = pathFinder.FindPath(start, goal);
        return pfsPath;
    }

    Vector2[] ScatterFinishPointsOfOrderForSelectedUnits(Vector2 CentralFinishPoint)
    {
        Vector2[] ScatteredPoints = ScatterFinishPoints(
            SelectedUnits.Select(a => new Vector2(a.transform.position.x, a.transform.position.y)).ToArray(),
            CentralFinishPoint
            );
        return ScatteredPoints;
    }

    Vector2[] ScatterFinishPoints(Vector2[] StartPoints, Vector2 FinishPoint)
    {
        if (StartPoints == null) return new Vector2[0];
        if (StartPoints.Count() == 0) return new Vector2[0];
        Vector2[] U = StartPoints;
        Vector2 Fc = FinishPoint;
        Vector2[] F = new Vector2[U.Count()];
        Vector2 Uc = new Vector2(U.Average(a => a.x), U.Average(a => a.y));
        Vector2 w = Fc - Uc;
        for (int i = 0; i < U.Count(); i++) F[i] = U[i] + w;
        //F vector is ready but it's not normalized. Let's check if it needs normalizing.
        //First, we get vectors from initial center to specific units locations:
        Vector2[] dU = new Vector2[U.Count()];
        for (int i = 0; i < U.Count(); i++) dU[i] = U[i] - Uc;
        //Second thing is average magnitude of dU
        float amdU = dU.Average(a => a.magnitude);
        if (amdU > maxAverageScatterDistance) //this is the condition showing if vector needs normalizing
        {
            float q = maxAverageScatterDistance / amdU;
            Vector2[] dF = new Vector2[U.Count()];
            for (int i = 0; i < U.Count(); i++)
            {
                dF[i] = q * (F[i] - Fc); //multiplying by q is actually normalizing of distance vectors
                F[i] = Fc + dF[i]; //now applying normalization to finish positions
            }
        }
        return F;
    }
}
