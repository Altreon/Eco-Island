using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Hide : SteeringBehavior
{
    private static Hide instance;
    public static Hide Instance {
        get{
            if(instance == null){
                instance = new Hide();
            }

            return instance;
        }
    }

    public Hide () : base(eSteeringBehavior.Hide){}

    public override void Enter (Agent agent) {
        base.Enter(agent);
        agent.Steering.SetDestination(HideNextPosition(agent));
    }

    public override void Update (Agent agent) {
        //TODO temp fixe
        if(agent.Steering.Evades.Count == 0){
            agent.Steering.Behavior = eSteeringBehavior.Wander;
            return;
        }

        agent.Steering.SetDestination(HideNextPosition(agent));
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }

    private Vector3 HideNextPosition (Agent agent) {
        //TODO Config
        IEnumerable<DataObstacle> obstacles = agent.Memory.Obstacles.Read().Where(data => MaxColliderSize(data.collider) >= 2f);
        //IEnumerable<DataObstacle> obstacles = agent.Memory.Obstacles.Read().Where(data => true);
        int obstaclesCount = obstacles.Count();
        List<Agent> evades = agent.Steering.Evades;
        if(obstaclesCount == 0 || evades.Count == 0) return agent.transform.position;

        Agent closestAgent = null;
        float closestDistance = Mathf.Infinity;
        foreach(Agent evade in evades){
            if(!evade || !evade.gameObject.activeSelf) continue;
            float distance = Vector3.Distance(agent.transform.position, evade.transform.position);

            if(distance < closestDistance){
                closestAgent = evade;
                closestDistance = distance;
            }
        }

        if(!closestAgent){
            agent.Steering.Behavior = eSteeringBehavior.Wander;
            return agent.transform.position;
        }

        Vector3 pursuerPosition = closestAgent.transform.position;

        Vector3[] hidePositions = new Vector3[obstaclesCount];
        int i = 0;
        foreach(DataObstacle data in obstacles){
            Collider collider = data.collider;

            Vector3 between = collider.transform.position - pursuerPosition;
            Vector3 dir = between.normalized;

            hidePositions[i] = pursuerPosition + between + dir * MaxColliderSize(collider);

            if(agent.Debug) Debug.DrawLine(agent.transform.position, hidePositions[i], Color.magenta);

            ++i;
        }


        Vector3 closestHidePosition = hidePositions[0];
        closestDistance = Vector3.Distance(agent.transform.position, closestHidePosition);
        for(i = 1; i < hidePositions.Length; ++i){
            float distance = Vector3.Distance(agent.transform.position, hidePositions[i]);

            if(distance < closestDistance && Vector3.Angle(pursuerPosition - agent.transform.position, hidePositions[i] - agent.transform.position) >= 90f){
                closestHidePosition = hidePositions[i];
                closestDistance = distance;
            }
        }

        return closestHidePosition;
    }

    private float MaxColliderSize(Collider collider) {
        Vector3 size = collider.bounds.size;

        /*if(size.x >= size.y && size.x >= size.z) return size.x;
        if(size.y >= size.x && size.y >= size.z) return size.y;
        return size.z;*/

        return size.x >= size.z ? size.x : size.y;
    }
}
