using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalEatFood : Goal
{
    private Transform eated;

    private Action<bool> eatCallback;

    public GoalEatFood(Agent owner, Transform eated) : base(owner) {
        this.eated = eated;
    }

    public override void Activate () {
        base.Activate();

        if(!eated || !eated.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        eatCallback += Eat;
        owner.Creature.CreatureDoing.EatFood(eated.GetComponent<Food>(), eatCallback);
    }

    public override void Process () {
  
    }

    private void Eat (bool succes) {
        status = succes ? GoalStatus.Completed : GoalStatus.Failed;
    }

    public override void Terminate () {
        base.Terminate();
    }
}
