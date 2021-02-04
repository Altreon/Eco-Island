using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPursuitPlayer : Goal
{
    private bool chase = true;
    private float distance;
    //private float deltaDistance = 0;

    public GoalPursuitPlayer (Agent owner) : base(owner) {

    }

     public override void Activate () {
        base.Activate();

        owner.Steering.Behavior = eSteeringBehavior.PursuitPlayer;

        distance = Mathf.Abs(Vector3.Distance(owner.transform.position, Player.Instance.transform.position));
    }

    public override void Process () {
        /*float oldDistance = Mathf.Abs(Vector3.Distance(owner.transform.position, target.transform.position));
        if(oldDistance > distance){
            deltaDistance = 0;
        }else{
            deltaDistance += distance - oldDistance;
        }
        distance = oldDistance;

        if(deltaDistance > 0.1f){
            status = GoalStatus.Failed;
            return;
        }*/

        Vector3 playerPos = Player.Instance.transform.position;
        float lastSeeTime = owner.Memory.Player.lastSeeTime;

        if(lastSeeTime > Time.time - 1f) {
            if(!chase){
                chase = true;
                owner.Steering.Behavior = eSteeringBehavior.PursuitPlayer;
            }

            if(owner.Steering.Behavior != eSteeringBehavior.PursuitPlayer){
                status = GoalStatus.Failed;
                return;
            }

            float distance = Vector3.Distance(owner.transform.position, playerPos);
            if(distance < 2){
                status = GoalStatus.Completed;
            }else if(distance > owner.PerceptionConfig.viewRadius){
                status = GoalStatus.Failed;
            }
        }else{
            if(chase){
                chase = false;
                owner.Steering.SetDestination(playerPos);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }

            if(owner.Steering.IsArrivedToDestination){
                status = GoalStatus.Failed;
                return;
            }
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
