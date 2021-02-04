using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursuit : SteeringBehavior
{
    private static Pursuit instance;
    public static Pursuit Instance {
        get{
            if(instance == null){
                instance = new Pursuit();
            }

            return instance;
        }
    }

    public Pursuit () : base(eSteeringBehavior.Pursuit){}

    public override void Enter (Agent agent) {
        base.Enter(agent);

        //TODO temp fixe
        if(!agent.Steering.Target || !agent.Steering.Target.gameObject.activeSelf){
            agent.Steering.Behavior = eSteeringBehavior.Seek;
            return;
        }

        if(agent.Steering.Target.Steering.Target == agent){
            agent.Steering.SetDestination(agent.Steering.Target.transform.position);
        }else{
            agent.Steering.SetDestination(TargetPrediction(agent, agent.Steering.Target));
        }
    }

    public override void Update (Agent agent) {
        //TODO temp fixe
        if(!agent.Steering.Target || !agent.Steering.Target.gameObject.activeSelf){
            agent.Steering.Behavior = eSteeringBehavior.Seek;
            return;
        }

        if(agent.Steering.Target.Steering.Target == agent){
            agent.Steering.SetDestination(agent.Steering.Target.transform.position);
        }else{
            agent.Steering.SetDestination(TargetPrediction(agent, agent.Steering.Target));
        }
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }

    private Vector3 TargetPrediction (Agent pursuer, Agent pursued) {
        Vector3 toEvader = pursued.transform.position - pursuer.transform.position;

        float lookAheadTime = toEvader.magnitude / (pursuer.Steering.MaxSpeed + pursued.Steering.Velocity.magnitude / 2);

        return pursued.transform.position + pursued.Steering.Velocity * lookAheadTime;

    }
}
