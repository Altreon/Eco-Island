using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWander : Goal
{
    private float startTime;
    private Vector3 destination;

    public GoalWander(Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();
        owner.Steering.Behavior = eSteeringBehavior.Wander;

        destination = owner.Steering.Destination;
        startTime = Time.time;
    }

    public override void Process () {
        if(Time.time - startTime > 20f){
            owner.Steering.SetDestination(Wander.Instance.RandPos(owner));
            startTime = Time.time;
        }else if(destination != owner.Steering.Destination){
            destination = owner.Steering.Destination;
            startTime = Time.time;
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
