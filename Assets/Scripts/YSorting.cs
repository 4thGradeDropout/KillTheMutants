using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSorting : MonoBehaviour
{
    public int SortingOrderPerYUnit = 100;

    SpriteRenderer renderer;
    Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 closestPoint = collider.ClosestPoint(new Vector2(-1000, -1000));
        int newSortingOrder = -(int)(closestPoint.y * SortingOrderPerYUnit);

        renderer.sortingOrder = newSortingOrder;
    }
}
