using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLookAt : Goal
{
    private Agent target;

    public GoalLookAt (Agent owner, Agent target) : base(owner) {
        this.target = target;
    }

     public override void Activate () {
        base.Activate();

        if(!target || !target.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        owner.Steering.Target = target;
        owner.Steering.Behavior = eSteeringBehavior.LookAt;
    }

    public override void Process () {
        if(!target || !target.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
