using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : SteeringBehavior
{
    //TODO fichier config
    float rotateSpeed = 5;

    private static LookAtPlayer instance;
    public static LookAtPlayer Instance {
        get{
            if(instance == null){
                instance = new LookAtPlayer();
            }

            return instance;
        }
    }

    public LookAtPlayer () : base(eSteeringBehavior.LookAtPlayer){}

    public override void Enter (Agent agent) {
        base.Enter(agent);

        agent.Steering.SetDestination(agent.transform.position);
    }

    public override void Update (Agent agent) {
        Vector3 playerPos = Player.Instance.transform.position;

        Vector3 header = Vector3.ProjectOnPlane(agent.transform.forward, Vector3.up).normalized;
        Vector3 toTarget = Vector3.ProjectOnPlane(playerPos - agent.transform.position, Vector3.up).normalized;

        float angle = Vector3.SignedAngle(header, toTarget, Vector3.up);
        Quaternion targetRotation = agent.transform.rotation * Quaternion.Euler(0, angle, 0);

        agent.Steering.TargetRotation = targetRotation;

        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public override void Exit (Agent agent) {
        base.Exit(agent);
    }
}
