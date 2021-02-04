using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior
{
    public eSteeringBehavior BehaviorType {get; private set;}

    public SteeringBehavior (eSteeringBehavior behaviorType){
        BehaviorType = behaviorType;
    }

    public virtual void Enter (Agent agent) {
        if(agent.Debug){
            Debug.Log("AI-Steering : " + agent.gameObject.name + " Enter on steering behaviour " + BehaviorType);
        }
    }

    public abstract void Update (Agent agent);

    public virtual void Exit (Agent agent) {
        if(agent.Debug){
            Debug.Log("AI-Steering : " + agent.gameObject.name + " Exit on steering behaviour " + BehaviorType);
        }
    }
}
