using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSeek : Goal
{
    private Vector3 destination;

    public GoalSeek(Agent owner, Vector3 destination) : base(owner) {
        this.destination = destination;
    }

     public override void Activate () {
        base.Activate();

        owner.Steering.SetDestination(destination);
        owner.Steering.Behavior = eSteeringBehavior.Seek;
    }

    public override void Process () {}

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
