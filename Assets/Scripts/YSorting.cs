using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSorting : MonoBehaviour
{
    public GameObject Host;
    public int SortingOrderPerYUnit = 100;

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
        int newSortingOrder = -(int)(transform.position.y * SortingOrderPerYUnit);

        if (Host != null)
            if ((Host.name == "Letov" || Host.name == "Tower1") && Input.GetKey(KeyCode.E))
                Debug.Log($"{Host.name}: {newSortingOrder}");

        renderer.sortingOrder = newSortingOrder;
    }
}
