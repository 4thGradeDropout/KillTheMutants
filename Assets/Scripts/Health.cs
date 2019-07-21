using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float InitialHP = 95f;

    public float MaxHP = 95f;

    float CurHP { get; set; }

    Dying dyingSystem { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CurHP = InitialHP;
        dyingSystem = GetComponent<Dying>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit(float resultingDamage)
    {
        CurHP -= resultingDamage;
        var healthInfo = new HealthInfo
        {
            CurHP = this.CurHP,
            MaxHP = this.MaxHP
        };
        if (dyingSystem.ShouldBeDead(healthInfo))
            dyingSystem.Die();
        
    }
}
