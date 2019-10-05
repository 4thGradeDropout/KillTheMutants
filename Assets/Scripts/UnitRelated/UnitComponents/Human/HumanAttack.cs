﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttack : Attack
{
    protected virtual void Update()
    {
        if (Input.GetMouseButton(0) && !animationInProgress)
            PerformAttack();

        base.Update();
    }
}