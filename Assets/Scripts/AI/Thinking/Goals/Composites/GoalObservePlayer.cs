using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalObservePlayer : GoalComposite
{
    //TODO config
    float minDistToObserve = 2;
    float maxDistToObserve = 5;

    float timeStart = 0;
    private enum ObserveState {
        Pursuit,
        LookAt
    }

    private ObserveState evadeState = ObserveState.Pursuit;

    public GoalObservePlayer (Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();

        AddSubgoal(new GoalPursuitPlayer(owner));
        timeStart = Time.time;
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        float vigilanceConsideration = desirabilitiesConfig.DefensePlayerConsiderationMinVigilance;

        float vigilance = desirabilitiesConfig.DefensePlayerDesirabilityByAggressivity.Evaluate(owner.Creature.Traits.Vigilance.Value) * (1 / vigilanceConsideration);
        float distToObserve = Mathf.Lerp(minDistToObserve, maxDistToObserve, 1 - vigilance);

        bool playerClose = owner.Memory.Player.lastSeeTime > Time.time - 1f 
                           && Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < distToObserve;

        switch (evadeState) {
            case ObserveState.Pursuit :
                if(child.IsComplete){
                    AddSubgoal(new GoalLookAtPlayer(owner));
                    evadeState = ObserveState.LookAt;
                }else if(playerClose){
                    child.Abort();
                    AddSubgoal(new GoalLookAtPlayer(owner));
                    evadeState = ObserveState.LookAt;
                }
                break;
            
            case ObserveState.LookAt :
                if(!playerClose){
                    child.Abort();
                    AddSubgoal(new GoalPursuitPlayer(owner));
                    evadeState = ObserveState.Pursuit;
                }
                break;
        }

        if(Time.time - timeStart >= desirabilitiesConfig.ObservePlayerMaxTimeSpend) {
            status = GoalStatus.Completed;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;
        owner.Memory.Player.lastObserved = Time.time;

        base.Terminate();
    }
}
