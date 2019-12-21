﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MutantMovement : CharacterMovement
{
    GameObject CurrentTarget
    {
        get
        {
            return MutantAttack.CurrentTarget;
        }
    }

    MutantAttack MutantAttack
    {
        get
        {
            return attack as MutantAttack;
        }
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (attack.AnimationInProgress)
            return;

        Vector2 currentPos = rigidBody.position;

        Vector2 direction = GetDesiredDirection();
        direction = Vector2.ClampMagnitude(direction, 1);
        Vector2 movement = direction * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        renderer.SetDirection(movement);

        if (MovingNow())
            soundPlayer.TurnFootstepsOn();
        else
            soundPlayer.TurnFootstepsOff();

        rigidBody.MovePosition(newPos);
    }

    Vector2 GetDesiredDirection()
    {
        Vector2 enemyPos = CurrentTarget.transform.position;

        if (enemyPos.x == float.NaN || enemyPos.y == float.NaN)
            return new Vector2(float.NaN, float.NaN);

        Vector2 myPos = rigidBody.position;
        return enemyPos - myPos;
    }
}
