using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee : SteeringBehavior
{
    //TODO fichier config
    float fleeDistancePrevision = 5f;

    private static Flee instance;
    public static Flee Instance {
        get{
            if(instance == null){
                instance = new Flee();
            }

            return instance;
        }
    }

    public Flee () : base(eSteeringBehavior.Flee){}

    public override void Enter (Agent agent) {
        base.Enter(agent);
        agent.Steering.SetDestination(EvadeNextPosition(agent));
    }

    public override void Update (Agent agent) {
        agent.Steering.SetDestination(EvadeNextPosition(agent));
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }

    private Vector3 EvadeNextPosition (Agent agent) {
        Vector3 fleeDirection = Vector3.zero;

        foreach(Agent evade in agent.Steering.Evades){
            if(!evade || !evade.gameObject.activeSelf) continue;
            Vector3 dir = agent.transform.position - evade.transform.position;
            fleeDirection += dir.normalized / dir.magnitude;
        }

        fleeDirection = (fleeDirection / agent.Steering.Evades.Count).normalized;

        NavMeshHit hit;
        float angle = 0;
        float angleIter = 1;
        bool found = true;
        while(!NavMesh.SamplePosition(agent.transform.position + Quaternion.AngleAxis(angle, Vector3.up) * fleeDirection * fleeDistancePrevision, out hit, 2f, NavMesh.AllAreas)
              && !NavMesh.SamplePosition(agent.transform.position + Quaternion.AngleAxis(-angle, Vector3.up) * fleeDirection * fleeDistancePrevision, out hit, 2f, NavMesh.AllAreas)){

            angle += angleIter;
            if(angle >= 180){
                found = false;
                break;
            }
        }

        Vector3 nextPos = agent.transform.position + fleeDirection * fleeDistancePrevision;
        return found ? hit.position : nextPos;
    }
}
