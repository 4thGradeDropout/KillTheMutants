using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMark : MonoBehaviour
{
    public int MaxVisibleStateCounter = 50;
    public float ZCoordinate = -0.02f;

    int visibleStateCounter = 0;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            visibleStateCounter++;
            if (visibleStateCounter > MaxVisibleStateCounter)
            {
                gameObject.SetActive(false);
                visibleStateCounter = 0;
            }
        }
    }

    public void InstantlyMoveTo(Vector3 coords)
    {
        transform.position = coords;
    }
}
