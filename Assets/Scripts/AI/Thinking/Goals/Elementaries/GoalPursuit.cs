using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPursuit : Goal
{
    private Agent target;

    private bool chase = true;
    private float distance;
    //private float deltaDistance = 0;

    public GoalPursuit (Agent owner, Agent target) : base(owner) {
        this.target = target;
    }

     public override void Activate () {
        base.Activate();

        if(!target || !target.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        owner.Steering.Target = target;
        owner.Steering.Behavior = eSteeringBehavior.Pursuit;

        distance = Mathf.Abs(Vector3.Distance(owner.transform.position, target.transform.position));
    }

    public override void Process () {
        if(!target || !target.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

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

        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        DataCreature targetData = null;

        foreach(DataCreature data in creatures){
            if(!data.creature || !data.creature.gameObject.activeSelf) continue;
            
            if(data.creature.agentCreature == target){
                targetData = data;
                break;
            }
        }

        if(targetData == null){
            status = GoalStatus.Failed;
            return;
        }

        if(targetData.RegistrationDate > Time.time - 1f) {
            if(!chase){
                chase = true;
                owner.Steering.Target = target;
                owner.Steering.Behavior = eSteeringBehavior.Pursuit;
            }

            if(owner.Steering.Behavior != eSteeringBehavior.Pursuit){
                status = GoalStatus.Failed;
                return;
            }

            float distance = Vector3.Distance(owner.transform.position, target.transform.position);
            if(distance < 2){
                status = GoalStatus.Completed;
            }else if(distance > owner.PerceptionConfig.viewRadius){
                status = GoalStatus.Failed;
            }
        }else{
            if(chase){
                chase = false;
                owner.Steering.SetDestination(targetData.lastPos);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }

            if(owner.Steering.IsArrivedToDestination){
                status = GoalStatus.Failed;
                return;
            }
        }
    }

    public override void Terminate () {
        owner.Steering.Target = null;
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
