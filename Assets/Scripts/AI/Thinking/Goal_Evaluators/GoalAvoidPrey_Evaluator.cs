using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalAvoidPrey_Evaluator : Goal_Evaluator
{
    private static GoalAvoidPrey_Evaluator instance;
    public static GoalAvoidPrey_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalAvoidPrey_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        //TODO TEMP
        if(agent.Creature.Traits.Carnivorous < 0.5f){
            return 0;
        }
        if(agent.Steering.Behavior != eSteeringBehavior.Seek && agent.Steering.Behavior != eSteeringBehavior.Wander && agent.Thinking.ActiveGoal?.GetType() != typeof(GoalAvoidPrey)){
            return 0;
        }

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        List<int> preys = new List<int>(agent.Memory.Species.GetByKey(agent.Creature.SpecieID).CarnivorousFoods.Select(food => food.preyID));
        IReadOnlyCollection<DataCreature> creatures = agent.Memory.Creatures.Read();
        float minDistanceFromPrey = -1;
        Creature closestPrey = null;

        foreach(DataCreature data in creatures){
            Agent otherAgent = data.creature?.agentCreature;
            if(!otherAgent || !agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 2f) continue;

            float distance = Vector3.Distance(agent.transform.position, otherAgent.transform.position);

            bool isPrey = preys.Contains(otherAgent.Creature.SpecieID);
            if(isPrey && (minDistanceFromPrey == -1 || distance < minDistanceFromPrey)){
                minDistanceFromPrey = distance;
                closestPrey = otherAgent.Creature;
            }
        }

        if(minDistanceFromPrey == -1) return 0;

        float desirability = desirabilitiesConfig.AvoidPreyDesirabilityByDistance.Evaluate(1 - minDistanceFromPrey / desirabilitiesConfig.AvoidPreyConsiderationMaxDistance)
                           * desirabilitiesConfig.AvoidPreyDesirabilityTwicker
                           * (1 / ForceEvaluation.Evaluate(agent, closestPrey));

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalAvoidPrey)) desirability *= desirabilitiesConfig.AvoidPreyConfirmationBias; 

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalAvoidPrey(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalAvoidPrey);
    }
}
