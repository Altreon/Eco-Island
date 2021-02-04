using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursuitPlayer : SteeringBehavior
{
    private static PursuitPlayer instance;
    public static PursuitPlayer Instance {
        get{
            if(instance == null){
                instance = new PursuitPlayer();
            }

            return instance;
        }
    }

    public PursuitPlayer () : base(eSteeringBehavior.PursuitPlayer){}

    public override void Enter (Agent agent) {
        base.Enter(agent);

        agent.Steering.SetDestination(TargetPrediction(agent));
    }

    public override void Update (Agent agent) {
        agent.Steering.SetDestination(TargetPrediction(agent));
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }

    private Vector3 TargetPrediction (Agent pursuer) {
        Player player = Player.Instance;
        Vector3 playerPos = player.transform.position;
        Vector3 playerVel = player.PlayerController.Velocity;
        Vector3 toEvader = playerPos - pursuer.transform.position;

        float lookAheadTime = toEvader.magnitude / (pursuer.Steering.MaxSpeed + playerVel.magnitude);

        return playerPos + playerVel * lookAheadTime;

    }
}
