using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSleep : Goal
{
    public GoalSleep(Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();

        owner.Steering.Behavior = eSteeringBehavior.Idle;

        owner.Creature.MetabolismActive = false;
        owner.Creature.BodyShader.JigleSleep = true;
        owner.Perception.ViewActive = false;
    }

    public override void Process () {
        
    }

    public override void Terminate () {
        owner.Creature.MetabolismActive = true;
        owner.Creature.BodyShader.JigleSleep = false;
        owner.Perception.ViewActive = true;

        base.Terminate();
    }
}
