using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEvade_Evaluator : Goal_Evaluator
{
    private static GoalEvade_Evaluator instance;
    public static GoalEvade_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalEvade_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        //TODO TEMP
        if(agent.Creature.Traits.Carnivorous >= 0.5f){
            return 0;
        }

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        IReadOnlyCollection<DataCreature> creatures = agent.Memory.Creatures.Read();
        float minDistanceFromHostil = -1;
        Creature closestHostil = null;

        foreach(DataCreature data in creatures){
            Agent otherAgent = data.creature?.agentCreature;
            if(!otherAgent || !agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 2f) continue;

            float distance = Vector3.Distance(agent.transform.position, otherAgent.transform.position);

            bool isHostil = GoalEvade.CreatureIsHostil(agent, otherAgent.Creature);
            if(isHostil && (minDistanceFromHostil == -1 || distance < minDistanceFromHostil)){
                minDistanceFromHostil = distance;
                closestHostil = otherAgent.Creature;
            }
        }

        if(minDistanceFromHostil == -1) return 0;

        float desirability = desirabilitiesConfig.EvadeDesirabilityByDistance.Evaluate(1 - minDistanceFromHostil / desirabilitiesConfig.EvadeConsiderationMaxDistance)
                           * desirabilitiesConfig.EvadeDesirabilityByAggressivity.Evaluate(agent.Creature.Traits.Aggressivity.Value)
                           * desirabilitiesConfig.EvadeDesirabilityTwicker
                           * (1 / ForceEvaluation.Evaluate(agent, closestHostil));

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalEvade)) desirability *= desirabilitiesConfig.EvadeConfirmationBias; 

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalEvade(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalEvade);
    }
}
