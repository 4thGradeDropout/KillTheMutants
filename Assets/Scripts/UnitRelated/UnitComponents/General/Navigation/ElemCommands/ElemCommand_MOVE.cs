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

    /// <summary>
    /// Счётчик количества фреймов, в течение которых юнит не двигался, 
    /// т.е., вероятнее всего, где-то "зацепился".
    /// </summary>
    int FramesWithoutMoving { get; set; } = 0;

    int MaxExpectedFramesWithoutMoving { get; set; } = 100;

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
            if (FramesWithoutMoving != 0)
            {
                FramesWithoutMoving = 0;
            }
        }
        else
        {
            FramesWithoutMoving++;
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

        if (FramesWithoutMoving >= MaxExpectedFramesWithoutMoving)
        {
            //Юнит застрял!
            //Чтобы это исправить, попробуем отдать приказ заново,
            //в ту же точку финиша, что и раньше.
            Debug.Log("Unit STUCK! Order remaking initiated...");
            StopAndRemakeOrder();
        }
    }

    /// <summary>
    /// Заставляет навигатор отбросить все текущие приказы, 
    /// перерассчитать путь до цели и начать выполнять этот новый путь
    /// </summary>
    protected void StopAndRemakeOrder()
    {
        ForcedStop = true;
        Host.StopAndRemakeOrder();
    }

    public static float GoalCompletionDistance { get; set; }
}
