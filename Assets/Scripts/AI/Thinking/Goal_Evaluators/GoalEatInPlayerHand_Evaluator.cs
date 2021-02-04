using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEatInPlayerHand_Evaluator : Goal_Evaluator
{
    private static GoalEatInPlayerHand_Evaluator instance;
    public static GoalEatInPlayerHand_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalEatInPlayerHand_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        if(agent.Creature.Traits.Carnivorous > 0.5f) return 0;
        if(Player.Instance.IsDie) return 0;
        if(!IsPlayerWantGiveFood()) return 0;

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        bool playerDetected = agent.Memory.Player.lastSeeTime > Time.time - 2f;
        if(!playerDetected) return 0;

        float distanceWithPlayer = Vector3.Distance(agent.transform.position, Player.Instance.transform.position);

        float desirability = GoalHungry_Evaluator.Instance.CalculateDesirability(agent, requested)
                           * desirabilitiesConfig.EatInPlayerHandDesirabilityByAggressivity.Evaluate(agent.Creature.Traits.Aggressivity.Value)
                           * desirabilitiesConfig.EatInPlayerHandDesirabilityByVigilance.Evaluate(agent.Creature.Traits.Vigilance.Value)
                           * desirabilitiesConfig.EatInPlayerHandDesirabilityTwicker;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalEatInPlayerHand)) desirability *= desirabilitiesConfig.EatInPlayerHandConfirmationBias; 

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalEatInPlayerHand(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalEatInPlayerHand);
    }

    public bool IsPlayerWantGiveFood () {
        return Player.Instance.PlayerPickAndDrop.IsHandleFood && Player.Instance.PlayerController.Velocity.magnitude < 1.5f;
    }
}
