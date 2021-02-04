using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSteering : MonoBehaviour
{
    public Agent idleAgent;
    public Agent wanderAgent;
    public Agent seekAgent;
    public Agent pursuitAgent;
    public Agent pursuitAgent2;
    public Agent fleeAgent;
    public Agent hideAgent;
    public Agent lookAtAgent;

    private Vector2 inputDirection;

    public void Start () {
        if(idleAgent) idleAgent.Steering.Behavior = eSteeringBehavior.Idle;

        wanderAgent.Steering.Behavior = eSteeringBehavior.Wander;

        if(seekAgent) seekAgent.Steering.SetDestination(new Vector3(0, 0, 15));
        if(seekAgent) seekAgent.Steering.Behavior = eSteeringBehavior.Seek;

        if(pursuitAgent) pursuitAgent.Steering.Target = fleeAgent;
        if(pursuitAgent) pursuitAgent.Steering.Behavior = eSteeringBehavior.Pursuit;

        if(pursuitAgent2) pursuitAgent2.Steering.Target = hideAgent;
        if(pursuitAgent2) pursuitAgent2.Steering.Behavior = eSteeringBehavior.Pursuit;

        if(fleeAgent) fleeAgent.Steering.Evades.Add(pursuitAgent);
        if(fleeAgent) fleeAgent.Steering.Behavior = eSteeringBehavior.Flee;

        if(hideAgent) hideAgent.Steering.Evades.Add(pursuitAgent2);
        if(hideAgent) hideAgent.Steering.Behavior = eSteeringBehavior.Hide;

        if(lookAtAgent) lookAtAgent.Steering.Target = seekAgent;
        if(lookAtAgent) lookAtAgent.Steering.Behavior = eSteeringBehavior.LookAt;
    }

    private void Update () {
        if(!seekAgent) return;
        
        if(seekAgent.Steering.Behavior != eSteeringBehavior.Seek) {
            seekAgent.Steering.SetDestination(new Vector3(0, 0, -seekAgent.Steering.Destination.z));
            seekAgent.Steering.Behavior = eSteeringBehavior.Seek;
        }
    }
}