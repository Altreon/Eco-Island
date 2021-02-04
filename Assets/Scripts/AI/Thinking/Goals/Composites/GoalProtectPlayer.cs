using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalProtectPlayer : GoalComposite
{
    private enum ProtectState {
        Start,
        Pursuit,
        Attack,
    }

    private ProtectState protectState = ProtectState.Start;

    public GoalProtectPlayer (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child != null && child.IsInactive){
            child.Activate();
        }

        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        Agent aggresor = null;
        float distanceToAggresor = Mathf.Infinity;
        foreach(DataCreature data in creatures){
            if(!data.creature) continue;
            Agent agent = data.creature.agentCreature;

            if(!agent || !agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

            if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalTired)) continue;

            Goal goal = agent.Thinking.ActiveGoal;
            if(!(goal is GoalDefensePlayer)) continue;

            Goal subGoal = (goal as GoalComposite).GetActiveGoal();
            if(subGoal == null || (!(subGoal is GoalPursuitPlayer) && !(subGoal is GoalAttackPlayer))) continue;

            float distanceToAgent = Vector3.Distance(owner.transform.position, agent.transform.position);

            if(agent != owner && agent.Creature.SpecieID != owner.Creature.SpecieID && distanceToAgent < distanceToAggresor){
                aggresor = agent;
                distanceToAggresor = distanceToAgent;
            }
        }

        if(!aggresor){
            status = GoalStatus.Completed;
            return;
        }

        switch (protectState){
            case ProtectState.Start :
                AddSubgoal(new GoalPursuit(owner, aggresor));
                protectState = ProtectState.Pursuit;
                break;

            case ProtectState.Pursuit :
                if(child.IsComplete) {
                    AddSubgoal(new GoalAttack(owner, owner.Steering.Target.Creature));
                    protectState = ProtectState.Attack;
                }
                else if((owner.Steering.Behavior == eSteeringBehavior.Pursuit || owner.Steering.Behavior == eSteeringBehavior.Seek)
                    && aggresor != owner.Steering.Target && Vector3.Distance(owner.transform.position, aggresor.transform.position) + 5f < Vector3.Distance(owner.transform.position, owner.Steering.Destination)){
                    child.Abort();
                    AddSubgoal(new GoalPursuit(owner, aggresor));
                }
                break;
            case ProtectState.Attack :
                if(child.IsComplete || child.HasFailed) {
                    AddSubgoal(new GoalPursuit(owner, aggresor));
                    protectState = ProtectState.Pursuit;
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }
}
