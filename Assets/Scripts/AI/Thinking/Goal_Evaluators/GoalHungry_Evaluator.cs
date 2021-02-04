using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHungry_Evaluator : Goal_Evaluator
{
    private static GoalHungry_Evaluator instance;
    public static GoalHungry_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalHungry_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        float desirability = desirabilitiesConfig.HungryDesirabilityByStomack.Evaluate(1 - agent.Creature.Gauges.Hunger.Rate)
                           * desirabilitiesConfig.HungryDesirabilityByGluttony.Evaluate(agent.Creature.Traits.Gluttony.Value)
                           * desirabilitiesConfig.HungryDesirabilityTwicker;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalHungry)) desirability *= desirabilitiesConfig.HungryConfirmationBias;
        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalHungry(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalHungry);
    }
}
