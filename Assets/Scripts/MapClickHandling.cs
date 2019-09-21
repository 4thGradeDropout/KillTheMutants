using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClickHandling : MonoBehaviour
{
    SelectionManager selectionManager;
    OrderManager orderManager;

    List<UnitSelection> selectedUnits
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
        if (Input.GetMouseButtonDown(1) && selectionManager.SelectedUnits.Count > 0)
        {
            orderManager.MakeRightClickOrder();
        }
    }
}
