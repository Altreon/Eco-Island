using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalProtectPlayer_Evaluator : Goal_Evaluator
{
    private static GoalProtectPlayer_Evaluator instance;
    public static GoalProtectPlayer_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalProtectPlayer_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        if(Player.Instance.IsDie) return 0;
        
        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        if(agent.Creature.Traits.Vigilance.Value > desirabilitiesConfig.ProtectPlayerConsiderationMaxVigilance) return 0;

        bool playerDetected = agent.Memory.Player.lastSeeTime > Time.time - 1f;
        if(!playerDetected) return 0;

        IReadOnlyCollection<DataCreature> creatures = agent.Memory.Creatures.Read();
        float minDistanceFromHostil = -1;
        Creature closestHostil = null;

        foreach(DataCreature data in creatures){
            Agent otherAgent = data.creature?.agentCreature;
            if(!otherAgent || !otherAgent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

            if(otherAgent.Creature.SpecieID == agent.Creature.SpecieID) continue;

            if(otherAgent.Thinking.ActiveGoal?.GetType() == typeof(GoalTired)) continue;

            Goal goal = otherAgent.Thinking.ActiveGoal;
            if(!(goal is GoalDefensePlayer)) continue;

            Goal subGoal = (goal as GoalComposite).GetActiveGoal();
            if(subGoal == null || (!(subGoal is GoalPursuitPlayer) && !(subGoal is GoalAttackPlayer))) continue;

            float distance = Vector3.Distance(agent.transform.position, otherAgent.transform.position);

            if(minDistanceFromHostil == -1 || distance < minDistanceFromHostil){
                minDistanceFromHostil = distance;
                closestHostil = otherAgent.Creature;
            }
        }

        if(minDistanceFromHostil == -1) return 0;

        float desirability = desirabilitiesConfig.ProtectPlayerDesirabilityByAggressivity.Evaluate(agent.Creature.Traits.Aggressivity.Value)
                           * desirabilitiesConfig.ProtectPlayerDesirabilityByVigilance.Evaluate(agent.Creature.Traits.Vigilance.Value)
                           * desirabilitiesConfig.ProtectPlayerDesirabilityTwicker
                           * ForceEvaluation.Evaluate(agent, closestHostil);

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalProtectPlayer)) desirability *= desirabilitiesConfig.ProtectPlayerConfirmationBias; 

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalProtectPlayer(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalProtectPlayer);
    }
}
