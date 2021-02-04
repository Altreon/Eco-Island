using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalReproduction_Evaluator : Goal_Evaluator
{
    private static GoalReproduction_Evaluator instance;
    public static GoalReproduction_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalReproduction_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        if(agent.Creature.Age < 0.4f){
            return 0;
        }

        //TODO temporaire, pour éviter la surpopulation :
        /*if(SpeciesMetrics.Instance && SpeciesMetrics.Instance.IsReady){
            int nbCreatures = SpeciesMetrics.Instance.NbCreatures[agent.Creature.SpecieID];
            if(nbCreatures >= 18){
                return 0;
            }
        }*/

        int nbCreatures = CreatureFactory.Instance.AliveCreature.Count(c => c.SpecieID == agent.Creature.SpecieID);
        if(nbCreatures >= 18){
            return 0;
        }

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        float desirability = desirabilitiesConfig.ReproductionDesirabilityByTimer.Evaluate(1 - agent.Creature.Gauges.Reproduction.Rate)
                            * desirabilitiesConfig.ReproductionDesirabilityByLust.Evaluate(agent.Creature.Traits.Lust.Value)
                            * desirabilitiesConfig.ReproductionDesirabilityTwicker;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalReproduction) ||
           agent.Thinking.ActiveGoal?.GetType() == typeof(GoalReproduce)) desirability *= desirabilitiesConfig.ReproductionConfirmationBias;
        if(requested) desirability *= desirabilitiesConfig.ReproductionRequestBias;

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalReproduction(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalReproduction);
    }
}
