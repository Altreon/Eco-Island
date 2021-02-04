using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Goal
Représente un but que l'agent souhaite atteindre, et la manière de le réaliser
*/

public enum GoalStatus {
    Inactive,
    Active,
    Completed,
    Failed
}

public abstract class Goal
{
    protected Agent owner;
    protected GoalStatus status;

    public Goal (Agent owner) {
        this.owner = owner;
        status = GoalStatus.Inactive;
    }

    public virtual void Activate () {
        if(owner.Debug){
            Debug.Log("AI-Thinking : " + owner.gameObject.name + " Activate the goal " + GetType().Name);
        }

        status = GoalStatus.Active;
    }

    public abstract void Process ();

    public virtual void Terminate () {
        if(owner.Debug){
            Debug.Log("AI-Thinking : " + owner.gameObject.name + " Terminate the goal " + GetType().Name);
        }
    }

    public virtual void Abort () {
        status = GoalStatus.Failed;
    }

    public bool IsActive {
        get{
            return status == GoalStatus.Active;
        }
    }

    public bool IsInactive {
        get{
            return status == GoalStatus.Inactive;
        }
    }

    public bool IsComplete {
        get{
            return status == GoalStatus.Completed;
        }
    }

    public bool HasFailed {
        get{
            return status == GoalStatus.Failed;
        }
    }
}
