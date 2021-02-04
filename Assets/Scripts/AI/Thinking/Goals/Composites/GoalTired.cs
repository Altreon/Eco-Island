using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalTired : GoalComposite
{

    private enum TiredState {
        ReachNest,
        Sleep
    }
    public GoalTired (Agent owner) : base(owner) {}

    private TiredState tiredState = TiredState.ReachNest;

    public override void Activate () {
        base.Activate();

        AddSubgoal(new GoalReachNestForSleep(owner));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        switch (tiredState){
            case TiredState.ReachNest :
                if(child.HasFailed || child.IsComplete){
                    AddSubgoal(new GoalSleep(owner));
                    tiredState = TiredState.Sleep;
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }
}
