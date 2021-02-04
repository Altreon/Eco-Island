using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalIdle : Goal
{
    public GoalIdle(Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();

        owner.Steering.Behavior = eSteeringBehavior.Idle;
    }

    public override void Process () {
        
    }

    public override void Terminate () {
        base.Terminate();
    }
}
