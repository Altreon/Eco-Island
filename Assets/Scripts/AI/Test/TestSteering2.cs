using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSteering2 : MonoBehaviour
{
    public Agent[] enemiesAgents;


    private Vector2 inputDirection;

    public void Start () {
        foreach(Agent agent in enemiesAgents) {
            agent.Steering.Behavior = eSteeringBehavior.Wander;
        }
    }
}