using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSleep_Evaluator : Goal_Evaluator
{
    private static GoalSleep_Evaluator instance;
    public static GoalSleep_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalSleep_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        //float sleepDistance = Mathf.Abs((DayCycleManager.Instance.Cycle + 0.5f) % 1 - agent.Creature.Traits.Nocturnal.Value);
        float sleep = Mathf.Abs(((DayCycleManager.Instance.Cycle + agent.Creature.Traits.Nocturnal.Value) % 1)* 2 - 1);

        float desirability = desirabilitiesConfig.SleepDesirabilityByCycleTime.Evaluate(sleep)
                           * desirabilitiesConfig.SleepDesirabilityTwicker;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalTired)) desirability *= desirabilitiesConfig.SleepConfirmationBias;

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalTired(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalTired);
    }
}
