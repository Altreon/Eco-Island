using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLookAtPlayer : Goal
{

    public GoalLookAtPlayer (Agent owner) : base(owner) {

    }

     public override void Activate () {
        base.Activate();

        owner.Steering.Behavior = eSteeringBehavior.LookAtPlayer;
    }

    public override void Process () {
        
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
