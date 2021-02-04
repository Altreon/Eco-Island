using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSteering3 : MonoBehaviour
{
    public Agent fleeAgent;
    public Agent[] pursuitAgents;


    private Vector2 inputDirection;

    public void Start () {
        foreach(Agent agent in pursuitAgents) {
            agent.Steering.Target = fleeAgent;
            agent.Steering.Behavior = eSteeringBehavior.Pursuit;
            fleeAgent.Steering.Evades.Add(agent);
        }

        fleeAgent.Steering.Behavior = eSteeringBehavior.Flee;
    }
}
