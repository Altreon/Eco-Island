using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAttackPlayer : Goal
{
    //TODO fichier config
    private float distToFollow = 1.5f;

    
    //TODO en fait attendre fin de attack
    private float startTime;

    public GoalAttackPlayer(Agent owner) : base(owner) {
        
    }

    public override void Activate () {
        base.Activate();

        owner.Creature.CreatureDoing.AttackPlayer();

        startTime = Time.time;

        owner.Steering.Behavior = eSteeringBehavior.PursuitPlayer;
    }

    public override void Process () {
        if(Time.time - startTime > 0.5f){
            status = GoalStatus.Completed;
            return;
        }

        Vector3 playerPos = Player.Instance.transform.position;

        if(owner.Steering.Behavior == eSteeringBehavior.PursuitPlayer){
            if(Vector3.Distance(owner.transform.position, playerPos) < distToFollow){
                owner.Steering.Behavior = eSteeringBehavior.LookAt;
            }
        }else if(owner.Steering.Behavior == eSteeringBehavior.Idle){
            if(Vector3.Distance(owner.transform.position, playerPos) >= distToFollow){
                owner.Steering.Behavior = eSteeringBehavior.PursuitPlayer;
            }
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
