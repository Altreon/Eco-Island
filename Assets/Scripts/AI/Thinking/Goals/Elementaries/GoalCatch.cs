using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCatch : Goal
{
    private Transform target;

    public GoalCatch (Agent owner, Transform target) : base(owner) {
        this.target = target;
    }

     public override void Activate () {
        base.Activate();

        if(!target || !target.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        owner.Steering.Aim = target;
        owner.Steering.Behavior = eSteeringBehavior.Catch;
    }

    public override void Process () {
        if(!target || !target.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        float distance = Vector3.Distance(owner.transform.position, target.position);
        if(distance < 2){
            status = GoalStatus.Completed;
        }else if(distance > owner.PerceptionConfig.viewRadius){
            status = GoalStatus.Failed;
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}