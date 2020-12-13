using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapClickHandling : MonoBehaviour
{
    SelectionManager selectionManager;
    OrderManager orderManager;

    protected bool IsObstacleMap
    {
        get
        {
            return gameObject.name.ToLower().Contains("obstacle"); 
        }
    }

    protected List<UnitSelection> selectedUnits
    {
        get
        {
            return selectionManager.SelectedUnits;
        }
    }

    protected Tilemap Map
    {
        get
        {
            return GetComponent<Tilemap>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        orderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //single-click
        if (Input.GetMouseButton(0))
        {
            selectionManager.ClearSelection();
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            var mousePosition = GetMousePosition();
            var goalPoint = GetWorldCoords(mousePosition);
            Debug.Log($"Mouse at: {goalPoint}");
        }
        if (Input.GetMouseButtonDown(1) && selectionManager.SelectedUnits.Count > 0)
        {
            var mousePosition = GetMousePosition();
            var goalPoint = GetWorldCoords(mousePosition);

            if (IsObstacleMap)
            {
                Debug.Log($"Hit in obstacle tile at {goalPoint}...");
                MapScanner scanner = new MapScanner(Map);
                goalPoint = scanner.GetNearestEmptyPoint(goalPoint);
                Debug.Log($"Nearest empty point is {goalPoint}");
            }
            orderManager.MakeRightClickOrder(goalPoint);
        }
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
}
