using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public List<UnitSelection> SelectedUnits { get; set; }
    public List<UnitSelection> UnitsOnMap { get; set; }

    private Rect selectionRect;
    private Rect actualSelectionRect;

    private bool draw;
    private Vector2 startPos;
    private Vector2 endPos;

    private GUISkin selectionSkin;

    /// <summary>
    /// ONLY way to remove selection!
    /// </summary>
    public void ClearSelection()
    {
        SelectedUnits.ForEach(a => a.Deselect());
        SelectedUnits.Clear();
    }

    public void AddUnitToSelection(UnitSelection unitSelection)
    {
        unitSelection.Select();
        SelectedUnits.Add(unitSelection);
    }

    public void RemoveFromSelection(UnitSelection unitSelection)
    {
        unitSelection.Deselect();
        SelectedUnits.Remove(unitSelection);
    }

    public void SelectAllUnitsOfTypeInScreen(GameObject UnitOfNeededType)
    {
        selectionRect = new Rect(0, 0, Screen.width, Screen.height);
        var units_of_type = UnitsOnMap.Select(u => u.gameObject)
                                      .Where(a => UnitOfNeededType.name == a.name)
                                      .ToList();
        for (int i = 0; i < units_of_type.Count; i++)
        {
            Vector3 tmp3d = GetScreenCoords(units_of_type[i].transform.position);
            Vector2 tmp = new Vector2(tmp3d.x, tmp3d.y);
            UnitSelection curSelection = units_of_type[i].GetComponent<UnitSelection>() as UnitSelection;
            if (selectionRect.Contains(tmp) && !curSelection.Selected) // selecting if wasn't selected and on screen
                AddUnitToSelection(UnitsOnMap[i]);
            if (!selectionRect.Contains(tmp) && curSelection.Selected) // deselecting if was selected and not in box
                RemoveFromSelection(UnitsOnMap[i]);
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

    public Vector3 GetScreenCoords(Vector3 WorldCoords)
    {
        var t = Camera.main.WorldToScreenPoint(WorldCoords);
        t.z = -0.1f;
        return t;
    }

    void OnGUI()
    {
        GUI.skin = selectionSkin;
        GUI.depth = 99;

        if (Input.GetMouseButtonDown(0))
        {
            //start stretching a selection box
            startPos = Input.mousePosition;
            draw = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            draw = false;
        }

        if (draw)
        {
            endPos = Input.mousePosition;
            if (startPos == endPos) return;

            selectionRect = new Rect(
                            Mathf.Min(endPos.x, startPos.x),
                            Screen.height - Mathf.Max(endPos.y, startPos.y),
                            Mathf.Max(endPos.x, startPos.x) - Mathf.Min(endPos.x, startPos.x),
                            Mathf.Max(endPos.y, startPos.y) - Mathf.Min(endPos.y, startPos.y)
                            );
            actualSelectionRect = new Rect(
                            Mathf.Min(endPos.x, startPos.x),
                            Mathf.Min(endPos.y, startPos.y),
                            Mathf.Max(endPos.x, startPos.x) - Mathf.Min(endPos.x, startPos.x),
                            Mathf.Max(endPos.y, startPos.y) - Mathf.Min(endPos.y, startPos.y)
                            );

            GUI.Box(selectionRect, "");

            for (int j = 0; j < UnitsOnMap.Count; j++)
            {
                // calculating screen coords of unit based on world coords
                Vector3 tmp3d = GetScreenCoords(UnitsOnMap[j].transform.position);
                Vector2 tmp = new Vector2(tmp3d.x, tmp3d.y);

                if (actualSelectionRect.Contains(tmp) && !UnitsOnMap[j].Selected) // selecting if wasn't selected and in box
                {
                    AddUnitToSelection(UnitsOnMap[j]);
                    //Debug.Log("Selected " + UnitsOnMap[j].name);
                }
                if (!actualSelectionRect.Contains(tmp) && UnitsOnMap[j].Selected) // deselecting if was selected and not in box
                {
                    RemoveFromSelection(UnitsOnMap[j]);
                    //Debug.Log("Deselected " + UnitsOnMap[j].name);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //setting GUIskin for selection
        selectionSkin = Resources.Load("SelectionSkin") as GUISkin;

        List<object> temp = new List<object>();
        temp.AddRange(GameObject.FindObjectsOfType(typeof(GameObject))
                                .Where(go => go.name.Contains("Letov")));
        temp.ForEach(a => { UnitsOnMap.Add((a as GameObject).GetComponent<UnitSelection>()); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        SelectedUnits = new List<UnitSelection>();
        UnitsOnMap = new List<UnitSelection>();
    }
}
