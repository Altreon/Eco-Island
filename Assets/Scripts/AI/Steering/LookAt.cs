using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : SteeringBehavior
{
    //TODO fichier config
    //float rotateSpeed = 5;

    private static LookAt instance;
    public static LookAt Instance {
        get{
            if(instance == null){
                instance = new LookAt();
            }

            return instance;
        }
    }

    public LookAt () : base(eSteeringBehavior.LookAt){}

    public override void Enter (Agent agent) {
        base.Enter(agent);

        agent.Steering.SetDestination(agent.transform.position);
    }

    public override void Update (Agent agent) {
        if(!agent.Steering.Target || !agent.Steering.Target.gameObject.activeSelf){
            agent.Steering.Behavior = eSteeringBehavior.Idle;
            return;
        }

        Vector3 header = Vector3.ProjectOnPlane(agent.transform.forward, Vector3.up).normalized;
        Vector3 toTarget = Vector3.ProjectOnPlane(agent.Steering.Target.transform.position - agent.transform.position, Vector3.up).normalized;

        float angle = Vector3.SignedAngle(header, toTarget, Vector3.up);
        Quaternion targetRotation = agent.transform.rotation * Quaternion.Euler(0, angle, 0);

        agent.Steering.TargetRotation = targetRotation;

        //agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }
}
