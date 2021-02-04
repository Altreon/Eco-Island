using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHungry : GoalComposite
{
    public GoalHungry (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();

        if(owner.Creature.Traits.Carnivorous.Value > 0.5f){
            AddSubgoal(new GoalHunt(owner));
        }else{
            AddSubgoal(new GoalHarvest(owner));
        }
    }

    public override void Process () {
        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }
}
