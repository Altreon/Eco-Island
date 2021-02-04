using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalEvadePlayer : GoalComposite
{
    private enum EvadeState {
        Flee,
        Hide
    }

    private EvadeState evadeState = EvadeState.Flee;

    public GoalEvadePlayer (Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();

        AddSubgoal(new GoalFleePlayer(owner));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        bool playerClose = owner.Memory.Player.lastSeeTime > Time.time - 5f 
                           && Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.PerceptionConfig.viewRadius / 4;

        DataObstacle obstacle = owner.Memory.Obstacles.Read().FirstOrDefault(data => MaxColliderSize(data.collider) >= 2f);
        bool obstacleClose = obstacle != null;

        switch (evadeState) {
            case EvadeState.Flee :
                if(!playerClose && obstacleClose){
                    child.Abort();
                    AddSubgoal(new GoalHidePlayer(owner));
                    evadeState = EvadeState.Hide;
                }
                break;
            
            case EvadeState.Hide :
                if(playerClose){
                    child.Abort();
                    AddSubgoal(new GoalFleePlayer(owner));
                    evadeState = EvadeState.Flee;
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }

    private float MaxColliderSize(Collider collider) {
        Vector3 size = collider.bounds.size;

        /*if(size.x >= size.y && size.x >= size.z) return size.x;
        if(size.y >= size.x && size.y >= size.z) return size.y;
        return size.z;*/

        return size.x >= size.z ? size.x : size.y;
    }
}
