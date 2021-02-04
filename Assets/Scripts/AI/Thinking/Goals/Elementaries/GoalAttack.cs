using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAttack : Goal
{
    //TODO fichier config
    private float distToFollow = 1.5f;

    private Creature attacked;
    
    //TODO en fait attendre fin de attack
    private float startTime;

    public GoalAttack(Agent owner, Creature attacked) : base(owner) {
        this.attacked = attacked;
    }

    public override void Activate () {
        base.Activate();

        if(!attacked || !attacked.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        owner.Creature.CreatureDoing.Attack(attacked);

        startTime = Time.time;

        owner.Steering.Target = attacked.agentCreature;
        owner.Steering.Behavior = eSteeringBehavior.Pursuit;
    }

    public override void Process () {
        if(!attacked || !attacked.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        if(Time.time - startTime > 0.5f){
            status = GoalStatus.Completed;
            return;
        }

        if(owner.Steering.Behavior == eSteeringBehavior.Pursuit){
            if(Vector3.Distance(owner.transform.position, attacked.transform.position) < distToFollow){
                owner.Steering.Behavior = eSteeringBehavior.LookAt;
            }
        }else if(owner.Steering.Behavior == eSteeringBehavior.Idle){
            if(Vector3.Distance(owner.transform.position, attacked.transform.position) >= distToFollow){
                owner.Steering.Behavior = eSteeringBehavior.Pursuit;
            }
        }
    }

    public override void Terminate () {
        owner.Steering.Target = null;
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
