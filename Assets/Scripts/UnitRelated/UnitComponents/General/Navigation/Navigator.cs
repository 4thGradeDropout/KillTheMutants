using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Navigator : MonoBehaviour
{
    public bool IsTurnedOn;

    public float GoalCompletionDistance = 1f;

    Queue<ElemCommand> NextCommands { get; set; }

    public ElemCommand CurrentlyExecutedCommand { get; protected set; }

    /// <summary>
    /// К какой точке должна привести последовательность команд, 
    /// выполняемая сейчас навигатором
    /// </summary>
    public Vector2 FinishPoint { get; set; }

    /// <summary>
    /// Координаты юнита, которому принадлежит навигатор
    /// </summary>
    public Vector2 UnitCoords
    {
        get
        {
            return gameObject.transform.position;
        }
    }

    protected OrderManager Order_Manager { get; set; } 

    public Vector2 UnitPosition
    {
        get
        {
            var collider = gameObject.GetComponent<BoxCollider2D>();
            return collider.bounds.center;
        }
    }

    public void Awake()
    {
        NextCommands = new Queue<ElemCommand>();
        var orderManagerGO = GameObject.Find("OrderManager");
        Order_Manager = orderManagerGO.GetComponent<OrderManager>();
    }

    public void FixedUpdate()
    {
        if (IsTurnedOn)
        {
            if (CurrentlyExecutedCommand == null 
             || CurrentlyExecutedCommand.GoalIsComplete
             || CurrentlyExecutedCommand.ForcedStop)
            {
                if (CurrentlyExecutedCommand != null)
                {
                    CurrentlyExecutedCommand.StopExecution();
                    CurrentlyExecutedCommand = null;
                }

                if (NextCommands.Count > 0)
                {
                    CurrentlyExecutedCommand = NextCommands.Dequeue();
                    CurrentlyExecutedCommand.StartExecution();
                }
            }
            if (CurrentlyExecutedCommand != null && CurrentlyExecutedCommand.InProgress)
            {
                CurrentlyExecutedCommand.ExecuteFrame();
            }
        }
    }

    public void TurnOn()
    {
        IsTurnedOn = true;
    }

    public void TurnOff()
    {
        IsTurnedOn = false;
    }

    public void ReceiveCommand(ElemCommand cmd)
    {
        NextCommands.Enqueue(cmd);
    }

    public void ReceiveCommands(List<ElemCommand> cmds)
    {
        cmds.ForEach(cmd => ReceiveCommand(cmd));
    }

    public void StopAndRemakeOrder()
    {
        NextCommands.Clear();
        CurrentlyExecutedCommand.StopExecution();
        CurrentlyExecutedCommand = null;
        Order_Manager.InstructNavigator(this);
    }
}
