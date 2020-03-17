using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElemCommand
{
    public ElemCommand(Navigator host)
    {
        Host = host;
        InProgress = false;
    }

    public Navigator Host { get; protected set; }

    public bool InProgress { get; protected set; }

    /// <summary>
    /// Если true, то выполнение команды немедленно прекращается
    /// </summary>
    public bool ForcedStop { get; protected set; } = false;

    public abstract string Name { get; }

    public virtual void StartExecution()
    {
        InProgress = true;
    }

    public virtual void StopExecution()
    {
        InProgress = false;
    }

    public abstract void ExecuteFrame();

    public abstract bool GoalIsComplete { get; }
}
