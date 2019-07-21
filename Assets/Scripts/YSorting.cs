using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSorting : MonoBehaviour
{
    public int IsometricRangePerYUnit = 100;

    SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int newSortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);

        #region DebugCatching
        //if (Input.GetKey(KeyCode.E))
        //{
        //    int sas = 100;
        //}
        #endregion

        renderer.sortingOrder = newSortingOrder;
    }
}
