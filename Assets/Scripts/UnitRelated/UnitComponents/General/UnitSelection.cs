using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    protected SelectionManager selectionManager;
    protected Dying dying;

    GameObject selectionMark;

    public Navigator navigator { get; set; }

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
        dying = GetComponent<Dying>();
        navigator = GetComponent<Navigator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        if (dying.Dead)
            return;

        selectionMark.SetActive(true);
        Selected = true;
    }

    public void Deselect()
    {
        if (dying.Dead)
            return;
        if (selectionMark != null)
            selectionMark.SetActive(false);
        Selected = false;
    }

    public void OneClickActions()
    {
        //Debug.Log("Single click actions");
        selectionManager.ClearSelection();
        selectionManager.AddUnitToSelection(this);
    }

    public void DoubleClickActions()
    {
        //Debug.Log("Double click actions");
        selectionManager.SelectAllUnitsOfTypeInScreen(this.gameObject);
    }

    
}
