using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBreed : Goal
{
    private Creature partenaire;
    
    //TODO en fait attendre fin de eat
    private float startTime;

    public GoalBreed(Agent owner, Creature partenaire) : base(owner) {
        this.partenaire = partenaire;
    }

    public override void Activate () {
        base.Activate();

        if(!partenaire || !partenaire.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        owner.Steering.Target = partenaire.agentCreature;
        owner.Steering.Behavior = eSteeringBehavior.LookAt;

        owner.Creature.CreatureDoing.Breed(partenaire);

        //TODO in the creature action
        owner.Creature.Gauges.Reproduction.Rate = 1f;

        startTime = Time.time;
    }

    public override void Process () {
        if(Time.time - startTime > 2f){
            status = GoalStatus.Completed;
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
