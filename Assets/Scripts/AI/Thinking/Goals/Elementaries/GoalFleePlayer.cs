using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFleePlayer : Goal
{
    public GoalFleePlayer (Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();
        
        owner.Steering.Behavior = eSteeringBehavior.FleePlayer;

        Revision.Instance.ReviseCreatureEvadePlayer(owner, owner.Creature);
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
