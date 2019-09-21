using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying : MonoBehaviour
{
    public bool InitiallyDead;

    public bool Dead { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Die()
    {
        Debug.Log("Now I'm a dead man!");
    }

    public virtual bool ShouldBeDead(HealthInfo info)
    {
        bool res = info.CurHP <= 0f;
        if (res)
            Die();
        return res;
    }
}

public class HealthInfo
{
    public float CurHP { get; set; }

    public float MaxHP {get; set; }
}
