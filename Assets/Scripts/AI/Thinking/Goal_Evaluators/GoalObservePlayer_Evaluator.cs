using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObservePlayer_Evaluator : Goal_Evaluator
{
    private static GoalObservePlayer_Evaluator instance;
    public static GoalObservePlayer_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalObservePlayer_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        if(Player.Instance.IsDie) return 0;
        
        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        if(agent.Creature.Traits.Vigilance.Value > desirabilitiesConfig.ObservePlayerConsiderationMaxVigilance) return 0;

        bool playerDetected = agent.Memory.Player.lastSeeTime > Time.time - 1f;
        if(!playerDetected) return 0;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalEatInPlayerHand)) return 0;

        if(agent.Thinking.ActiveGoal?.GetType() != typeof(GoalObservePlayer)
           && agent.Memory.Player.lastObserved > Time.time - desirabilitiesConfig.ObservePlayerMinTimeRepeat)
           return 0;

        float distanceWithPlayer = Vector3.Distance(agent.transform.position, Player.Instance.transform.position);

        float desirability = desirabilitiesConfig.ObservePlayerDesirabilityByDistance.Evaluate(1 - distanceWithPlayer / desirabilitiesConfig.ObservePlayerConsiderationMaxDistance)
                           * desirabilitiesConfig.ObservePlayerDesirabilityByCuriosiry.Evaluate(agent.Creature.Traits.Curiosity.Value)
                           * desirabilitiesConfig.ObservePlayerDesirabilityByVigilance.Evaluate(agent.Creature.Traits.Vigilance.Value)
                           * desirabilitiesConfig.ObservePlayerDesirabilityTwicker;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalObservePlayer)) desirability *= desirabilitiesConfig.ObservePlayerConfirmationBias; 

        if(GoalEatInPlayerHand_Evaluator.Instance.IsPlayerWantGiveFood()) desirability *= desirabilitiesConfig.ObservePlayerDesirabilityTwickerWithFoodHolder;

        if(agent.Memory.Player.lastTimeFoodGive > Time.time - 10f) desirability *= desirabilitiesConfig.ObservePlayerDesirabilityTwickerWithGiveFood;

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalObservePlayer(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalObservePlayer);
    }
}
