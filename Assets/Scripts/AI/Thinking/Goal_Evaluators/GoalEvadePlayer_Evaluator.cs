using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEvadePlayer_Evaluator : Goal_Evaluator
{
    private static GoalEvadePlayer_Evaluator instance;
    public static GoalEvadePlayer_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalEvadePlayer_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        if(Player.Instance.IsDie) return 0;

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        if(agent.Creature.Traits.Vigilance.Value < desirabilitiesConfig.EvadePlayerConsiderationMinVigilance) return 0;

        bool playerDetected = agent.Memory.Player.lastSeeTime > Time.time - 2f;
        if(!playerDetected) return 0;

        float distanceWithPlayer = Vector3.Distance(agent.transform.position, Player.Instance.transform.position);

        float desirability = desirabilitiesConfig.EvadePlayerDesirabilityByDistance.Evaluate(1 - distanceWithPlayer / desirabilitiesConfig.EvadePlayerConsiderationMaxDistance)
                           * desirabilitiesConfig.EvadePlayerDesirabilityByAggressivity.Evaluate(agent.Creature.Traits.Aggressivity.Value)
                           * desirabilitiesConfig.EvadePlayerDesirabilityByVigilance.Evaluate(agent.Creature.Traits.Vigilance.Value)
                           * desirabilitiesConfig.EvadePlayerDesirabilityTwicker
                           * (1 / ForceEvaluation.EvaluateAgainstPlayer(agent));

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalEvadePlayer)) desirability *= desirabilitiesConfig.EvadePlayerConfirmationBias;

        if(GoalEatInPlayerHand_Evaluator.Instance.IsPlayerWantGiveFood()) desirability *= desirabilitiesConfig.EvadePlayerDesirabilityTwickerWithFoodHolder;

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalEvadePlayer(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalEvadePlayer);
    }
}
