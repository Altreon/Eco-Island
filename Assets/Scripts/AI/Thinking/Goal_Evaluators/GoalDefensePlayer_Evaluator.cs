using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDefensePlayer_Evaluator : Goal_Evaluator
{
    private static GoalDefensePlayer_Evaluator instance;
    public static GoalDefensePlayer_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalDefensePlayer_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        if(Player.Instance.IsDie) return 0;

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        if(agent.Creature.Traits.Vigilance.Value < desirabilitiesConfig.DefensePlayerConsiderationMinVigilance) return 0;

        bool playerDetected = agent.Memory.Player.lastSeeTime > Time.time - 2f;
        if(!playerDetected) return 0;

        if(agent.Thinking.ActiveGoal?.GetType() != typeof(GoalDefensePlayer)
           && agent.Memory.Player.lastObserved > Time.time - desirabilitiesConfig.DefensePlayerMinTimeRepeat)
           return 0;

        float distanceWithPlayer = Vector3.Distance(agent.transform.position, Player.Instance.transform.position);

        float desirability = desirabilitiesConfig.DefensePlayerDesirabilityByAggressivity.Evaluate(agent.Creature.Traits.Aggressivity.Value)
                           * desirabilitiesConfig.DefensePlayerDesirabilityByVigilance.Evaluate(agent.Creature.Traits.Vigilance.Value)
                           * desirabilitiesConfig.DefensePlayerDesirabilityTwicker
                           * ForceEvaluation.EvaluateAgainstPlayer(agent);

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalDefensePlayer)) desirability *= desirabilitiesConfig.DefensePlayerConfirmationBias; 

        if(GoalEatInPlayerHand_Evaluator.Instance.IsPlayerWantGiveFood()) desirability *= desirabilitiesConfig.DefensePlayerDesirabilityTwickerWithFoodHolder;

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalDefensePlayer(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalDefensePlayer);
    }
}
