using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElemCommand_MOVE : ElemCommand
{
    public ElemCommand_MOVE(Navigator host, Vector2 goal) : base(host)
    {
        GoalPosition = goal;
        MovementComponent = Host.GetComponent<CharacterMovement>();

        GoalCompletionDistance = host.GoalCompletionDistance;
    }

    public override string Name => "MOVE";

    Vector2 UnitPosition
    {
        get
        {
            return Host.UnitPosition;
        }
    }

    Vector2 GoalPosition { get; set; }

    CharacterMovement MovementComponent { get; set; }

    float CurrentDistance
    {
        get
        {
            return (GoalPosition - UnitPosition).magnitude;
        }
    }

    float PreviousDistance { get; set; }

    public override bool GoalIsComplete
    {
        get
        {
            return CurrentDistance < GoalCompletionDistance;
        }
    }

    public void RefreshPreviousDistance()
    {
        if (Math.Abs(CurrentDistance - PreviousDistance) > 0.1f)
        {
            //Debug.Log($"The distance to {GoalPosition} is {CurrentDistance}");
            PreviousDistance = CurrentDistance;
        }
    }

    public override void StartExecution()
    {
        base.StartExecution();
        MovementComponent.CurrentDirection = GoalPosition - UnitPosition;
    }

    public override void StopExecution()
    {
        base.StopExecution();
        MovementComponent.CurrentDirection = new Vector2(0,0);
    }

    public override void ExecuteFrame()
    {
        RefreshPreviousDistance();
    }

    public static float GoalCompletionDistance { get; set; }
}
