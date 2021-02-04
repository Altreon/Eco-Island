using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHidePlayer : Goal
{
    public GoalHidePlayer (Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();

        owner.Steering.Behavior = eSteeringBehavior.HidePlayer;
    }

    public override void Process () {
       bool playerDetected = owner.Memory.Player.lastSeeTime > Time.time - 5f;

        if(!playerDetected) status = GoalStatus.Completed;
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
