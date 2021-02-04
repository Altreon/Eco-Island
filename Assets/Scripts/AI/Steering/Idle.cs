using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : SteeringBehavior
{
    private static Idle instance;
    public static Idle Instance {
        get{
            if(instance == null){
                instance = new Idle();
            }

            return instance;
        }
    }

    public Idle () : base(eSteeringBehavior.Idle){}

    public override void Enter (Agent agent) {
        base.Enter(agent);

        agent.Steering.NavStop();
    }

    public override void Update (Agent agent) {
        
    }

    public override void Exit (Agent agent) {
        agent.Steering.NavStart();

        base.Exit(agent);
    }
}
