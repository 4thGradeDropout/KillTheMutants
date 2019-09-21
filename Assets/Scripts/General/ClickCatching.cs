using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCatching : MonoBehaviour
{
    private UnitSelection SelectionScript { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SelectionScript = gameObject.GetComponent<UnitSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        SelectionScript.ProcessMouseClick();
    }
}
