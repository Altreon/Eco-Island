using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : SteeringBehavior
{
    //TODO fichier config
    float wanderMaxDistance = 30f;

    private static Wander instance;
    public static Wander Instance {
        get{
            if(instance == null){
                instance = new Wander();
            }

            return instance;
        }
    }

    public Wander () : base(eSteeringBehavior.Wander){}

    public override void Enter (Agent agent) {
        base.Enter(agent);
        agent.Steering.SetDestination(RandPos(agent));
    }

    public override void Update (Agent agent) {
        if(agent.Steering.IsArrivedToDestination){
            agent.Steering.SetDestination(RandPos(agent));
        }
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }

    public Vector3 RandPos (Agent agent) {
        Vector2 agentPos2D = new Vector2(agent.transform.position.x, agent.transform.position.z);
        Vector2 randPos2D = agentPos2D + Random.insideUnitCircle * wanderMaxDistance;
        
        NavMeshHit hit;
        if(!NavMesh.SamplePosition(new Vector3(randPos2D.x, agent.transform.position.y, randPos2D.y), out hit, 8, NavMesh.AllAreas)){
            return agent.transform.position;
        }

        return hit.position;
    }
}
