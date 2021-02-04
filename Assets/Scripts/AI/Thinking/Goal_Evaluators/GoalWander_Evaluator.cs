using System.Collections;
using System.Collections.Generic;

public class GoalWander_Evaluator : Goal_Evaluator
{
    private static GoalWander_Evaluator instance;
    public static GoalWander_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalWander_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        return GameManager.Instance.DesirabilitiesConfig.WanderDesirabilityValue;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalWander(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalWander);
    }
}
