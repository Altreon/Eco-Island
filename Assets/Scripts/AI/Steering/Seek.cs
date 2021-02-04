using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Seek : SteeringBehavior
{
    private static Seek instance;
    public static Seek Instance {
        get{
            if(instance == null){
                instance = new Seek();
            }

            return instance;
        }
    }

    public Seek () : base(eSteeringBehavior.Seek){}

    public override void Enter (Agent agent) {
        base.Enter(agent);
        agent.Steering.SetDestination(agent.Steering.Destination);
    }

    public override void Update (Agent agent) {
        if(agent.Steering.IsArrivedToDestination){
            agent.Steering.Behavior = eSteeringBehavior.Idle;
        }
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }
}
