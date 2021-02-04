using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalEatCreature : Goal
{
    //TODO creature
    private Creature eated;

    private Action<bool> eatCallback;

    public GoalEatCreature(Agent owner, Creature eated) : base(owner) {
        this.eated = eated;
    }

    public override void Activate () {
        base.Activate();

        if(!eated || !eated.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        owner.Steering.Target = eated.agentCreature;
        owner.Steering.Behavior = eSteeringBehavior.LookAt;

        eatCallback += Eat;
        owner.Creature.CreatureDoing.Eat(eated, eatCallback);
    }

    public override void Process () {
  
    }

    private void Eat (bool succes) {
        status = succes ? GoalStatus.Completed : GoalStatus.Failed;
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
