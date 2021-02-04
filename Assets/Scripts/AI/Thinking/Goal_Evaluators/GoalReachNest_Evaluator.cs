using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalReachNest_Evaluator : Goal_Evaluator
{
    private static GoalReachNest_Evaluator instance;
    public static GoalReachNest_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalReachNest_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        DataNest dataNest = agent.Memory.Nests.Read().FirstOrDefault(data => data.nest.SpecieID == agent.Creature.SpecieID);
        if(dataNest == null) return 0;

        float distanceFromNest = Vector3.Distance(agent.transform.position, dataNest.nest.transform.position);
        if(distanceFromNest > desirabilitiesConfig.ReachNestConsiderationMaxDistance) distanceFromNest = desirabilitiesConfig.ReachNestConsiderationMaxDistance;

        float desirability = desirabilitiesConfig.ReachNestDesirabilityByDistance.Evaluate(distanceFromNest / desirabilitiesConfig.ReachNestConsiderationMaxDistance)
                           * desirabilitiesConfig.ReachNestDesirabilityTwicker;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalEvade)) desirability *= desirabilitiesConfig.ReachNestConfirmationBias; 

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalReachNest(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalReachNest);
    }
}
