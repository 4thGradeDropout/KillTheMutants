using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public float DoubleclickDelay = 0.1f; //this is how long in seconds to allow for a double click

    protected SelectionManager selectionManager;

    protected bool oneClick = false;
    protected float timerForDoubleClick = 0; //counter

    GameObject selectionMark;

    public bool Selected { get; protected set; }

    public Vector2 GO_Coords
    {
        get
        {
            return gameObject.transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        selectionManager = GameObject.Find("SelectionManager")
                                     .GetComponent<SelectionManager>();
        Transform ts = gameObject.transform.GetComponentInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "SelectionMark")
                selectionMark = t.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleDoubleClickInUpdate();
    }

    public void ProcessMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!oneClick) // first click no previous clicks
            {
                oneClick = true;
                timerForDoubleClick = Time.time; // save the current time
                OneClickActions();
            }
            else
            {
                oneClick = false; // found a double click, now reset
                DoubleClickActions();
            }
        }
    }

    public void Select()
    {
        selectionMark.SetActive(true);
        Selected = true;
    }

    public void Deselect()
    {
        selectionMark.SetActive(false);
        Selected = false;
    }

    void OneClickActions()
    {
        //Debug.Log("Single click actions");
        selectionManager.ClearSelection();
        selectionManager.AddUnitToSelection(this);
    }

    void DoubleClickActions()
    {
        //Debug.Log("Double click actions");
        selectionManager.SelectAllUnitsOfTypeInScreen(this.gameObject);
    }

    protected void HandleDoubleClickInUpdate()
    {
        //double click
        if (oneClick)
        {
            if ((Time.time - timerForDoubleClick) > DoubleclickDelay)
            {
                Debug.Log("Click counter reset");
                //basically if we're here then it's been too long and we want to reset the counter
                //so the next click is simply a single click and not a double click.
                oneClick = false;
            }
        }
    }
}
