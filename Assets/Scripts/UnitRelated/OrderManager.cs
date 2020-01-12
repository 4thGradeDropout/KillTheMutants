using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderManager : MonoBehaviour
{
    protected OrderMark orderMark;
    protected SelectionManager selectionManager;

    float maxAverageScatterDistance = 1.2f;

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

    public Vector3 GetMousePosition()
    {
        return new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
    }

    public Vector3 GetWorldCoords(Vector3 ScreenCoords)
    {
        var t = Camera.main.ScreenToWorldPoint(ScreenCoords);
        t.z = -0.1f;
        return t;
    }

    public void MakeRightClickOrder()
    {
        Vector3 mousePosition = GetMousePosition();
        //Debug.Log("Right-click at: " + mousePosition.ToString());
        Vector3 clickedPosition = GetWorldCoords(mousePosition);
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
            //Now find path to that point:
            var pathFinder = new PathFinder();
            var mapGenerator = GameObject.Find("ObstaclesMapGenerator").GetComponent<ObstaclesMapGenerator>();
            var converter = new TileCoordSystemConverter(mapGenerator.WorldSize, mapGenerator.CellSize);
            Point start = converter.UW_To_PFS(SelectedUnits[i].GO_Coords);
            Point goal = converter.UW_To_PFS(FinPoints[i]);
            var pfsPath = pathFinder.FindPath(start, goal);
            if (pfsPath == null) continue;
            //Получили путь в PFS. Теперь получим путь в US.
            var usPath = pfsPath.Select(p => converter.PFS_To_US(p)).ToList();
            //---------------------------------------------------------------------------------------
            //Debug.Log("НАЙДЕННЫЙ ПУТЬ: " + StringManipulation.ListToString(usPath));
            //---------------------------------------------------------------------------------------
            //А теперь путь в UW:
            var uwPath = usPath.Select(p => converter.US_To_UW(p)).ToList();
            uwPath.Remove(uwPath[0]);
            PathToCommandConverter pathToCmdConverter = new PathToCommandConverter();
            var unitNavigator = SelectedUnits[i].navigator;
            var commands = pathToCmdConverter.PathToCommands(uwPath, unitNavigator);
            unitNavigator.ReceiveCommands(commands);
        }
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
