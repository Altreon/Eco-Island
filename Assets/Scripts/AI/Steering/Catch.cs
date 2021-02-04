using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : SteeringBehavior
{
    private static Catch instance;
    public static Catch Instance {
        get{
            if(instance == null){
                instance = new Catch();
            }

            return instance;
        }
    }

    public Catch () : base(eSteeringBehavior.Catch){}

    public override void Enter (Agent agent) {
        base.Enter(agent);

        //TODO temp fixe
        if(!agent.Steering.Aim || !agent.Steering.Aim.gameObject.activeSelf){
            agent.Steering.Behavior = eSteeringBehavior.Seek;
            return;
        }

        agent.Steering.SetDestination(agent.Steering.Aim.position);
    }

    public override void Update (Agent agent) {
        //TODO temp fixe
        if(!agent.Steering.Aim || !agent.Steering.Aim.gameObject.activeSelf){
            agent.Steering.Behavior = eSteeringBehavior.Seek;
            return;
        }

        agent.Steering.SetDestination(agent.Steering.Aim.position);
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }

    //Pas utiliser pour l'instant, utiliser un rigidbody dans Aim si besoin de prédiction
    /*private Vector3 TargetPrediction (Agent pursuer, Agent pursued) {
        Vector3 toEvader = pursued.transform.position - pursuer.transform.position;

        float lookAheadTime = toEvader.magnitude / (pursuer.Steering.MaxSpeed + pursued.Steering.Velocity.magnitude / 2);

        return pursued.transform.position + pursued.Steering.Velocity * lookAheadTime;

    }*/
}
