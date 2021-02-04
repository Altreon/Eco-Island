using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDefense_Evaluator : Goal_Evaluator
{
    private static GoalDefense_Evaluator instance;
    public static GoalDefense_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalDefense_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        IReadOnlyCollection<DataCreature> creatures = agent.Memory.Creatures.Read();
        float minDistanceFromHostil = -1;
        Creature closestHostil = null;

        foreach(DataCreature data in creatures){
            Agent otherAgent = data.creature?.agentCreature;
            if(!otherAgent || !otherAgent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

            //bool attackSpecies = otherAgent.Steering.Target && otherAgent.Steering.Target.Creature.SpecieID == agent.Creature.SpecieID;
            //if(!attackSpecies) continue;

            if(otherAgent.Thinking.ActiveGoal?.GetType() == typeof(GoalTired)) return 0;

            float distance = Vector3.Distance(agent.transform.position, otherAgent.transform.position);

            bool isHostil = GoalEvade.CreatureIsHostil(agent, otherAgent.Creature);
            if(!isHostil) continue;

            if(minDistanceFromHostil == -1 || distance < minDistanceFromHostil){
                minDistanceFromHostil = distance;
                closestHostil = otherAgent.Creature;
            }
        }

        if(minDistanceFromHostil == -1) return 0;

        float desirability = desirabilitiesConfig.DefenseDesirabilityByAggressivity.Evaluate(agent.Creature.Traits.Aggressivity.Value)
                           * desirabilitiesConfig.DefenseDesirabilityTwicker
                           * ForceEvaluation.Evaluate(agent, closestHostil);

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalDefense)) desirability *= desirabilitiesConfig.DefenseConfirmationBias; 

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalDefense(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalDefense);
    }
}
