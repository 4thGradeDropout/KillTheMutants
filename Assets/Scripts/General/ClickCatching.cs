using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickCatching : MonoBehaviour
{
    public float DoubleclickDelay = 0.1f; //this is how long in seconds to allow for a double click

    protected UnitSelection SelectionScript { get; set; }
    protected OrderManager OrderMaking { get; set; }
    protected SelectionManager SelectionManager_ { get; set; }
    protected Tilemap Map { get; set; }

    protected bool oneClick = false;
    protected float timerForDoubleClick = 0; //counter

    // Start is called before the first frame update
    void Start()
    {
        SelectionScript = GetComponent<UnitSelection>();
        SelectionManager_ = GetComponent<SelectionManager>();

        var orderManagerGO = GameObject.Find("OrderManager");
        if (orderManagerGO != null)
        {
            OrderMaking = orderManagerGO.GetComponent<OrderManager>();
        }

        var gridGO = GameObject.Find("Grid");
        if (gridGO != null)
        {
            foreach (Transform child in gridGO.transform)
            {
                var curChildGO = child.gameObject;
                if (curChildGO.name.ToLower().Contains("obstacle"))
                {
                    Map = curChildGO.GetComponent<Tilemap>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleDoubleClickInUpdate();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProcessMouseLeftClick();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            ProcessMouseRightClick();
        }
    }

    protected void HandleDoubleClickInUpdate()
    {
        //double click
        if (oneClick)
        {
            if ((Time.time - timerForDoubleClick) > DoubleclickDelay)
            {
                //Debug.Log("Click counter reset");
                //basically if we're here then it's been too long and we want to reset the counter
                //so the next click is simply a single click and not a double click.
                oneClick = false;
            }
        }
    }

    public void ProcessMouseLeftClick()
    {
        if (!oneClick) // first click no previous clicks
        {
            oneClick = true;
            timerForDoubleClick = Time.time; // save the current time
            if (SelectionScript != null)
            {
                SelectionScript.OneClickActions();
            }
        }
        else
        {
            oneClick = false; // found a double click, now reset
            if (SelectionScript != null)
            {
                SelectionScript.DoubleClickActions();
            }
        }

    }

    public void ProcessMouseRightClick()
    {
        var mousePosition = GetMousePosition();
        var goalPoint = GetWorldCoords(mousePosition);

        Debug.Log($"Hit in {gameObject} at {goalPoint}...");
        
        MapScanner scanner = new MapScanner(Map);
        goalPoint = scanner.GetNearestEmptyPoint(goalPoint);
        Debug.Log($"Nearest empty point is {goalPoint}");
        OrderMaking.MakeRightClickOrder(goalPoint);
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
