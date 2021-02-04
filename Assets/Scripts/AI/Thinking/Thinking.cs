using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Thinking
Module de prise de décision, organise les différents buts que l'agent souhaite atteindre en plannifiant leur exécutions
*/

public class Thinking
{
    private Agent owner;
    private List<Goal_Evaluator> evaluators;

    private Goal activeGoal;
    public Goal ActiveGoal {
        get {return activeGoal;}
        set{
            if(activeGoal != null) ActiveGoal.Terminate();
            if(value != null) value.Activate();
            activeGoal = value;
       }
    }

    public Thinking (Agent owner) {
        this.owner = owner;

        evaluators = new List<Goal_Evaluator>();
        evaluators.Add(GoalWander_Evaluator.Instance);
        evaluators.Add(GoalEvade_Evaluator.Instance);
        evaluators.Add(GoalEvadePlayer_Evaluator.Instance);
        evaluators.Add(GoalHungry_Evaluator.Instance);
        evaluators.Add(GoalReproduction_Evaluator.Instance);
        evaluators.Add(GoalDefense_Evaluator.Instance);
        evaluators.Add(GoalDefensePlayer_Evaluator.Instance);
        evaluators.Add(GoalCommunication_Evaluator.Instance);
        evaluators.Add(GoalReachNest_Evaluator.Instance);
        evaluators.Add(GoalAvoidPrey_Evaluator.Instance);
        evaluators.Add(GoalObservePlayer_Evaluator.Instance);
        evaluators.Add(GoalSleep_Evaluator.Instance);
        evaluators.Add(GoalEatInPlayerHand_Evaluator.Instance);
        evaluators.Add(GoalProtectPlayer_Evaluator.Instance);

        //TODO retirer cette ajout temporaire
        /*DataSpecies data = owner.Memory.Species.GetByKey(owner.Creature.SpecieID);
        if(owner.Creature.Traits.Carnivorous > 0.5f){
            for(int i = 0; i <= 10; i++){
                if(i == owner.Creature.SpecieID) continue;
                data.addCarnivorousFood(new CarnivorousFood(i, Time.time));
            }
        }else{
            data.addHerbivorFood(new HerbivorFood(FoodType.Fruit, Time.time));
            data.addHerbivorFood(new HerbivorFood(FoodType.Vegetable, Time.time));
        }*/
    }

    public void Update () {
        if(ActiveGoal != null && (ActiveGoal.IsComplete || ActiveGoal.HasFailed)){
            ActiveGoal.Terminate();
            ActiveGoal = null;
        }

        /*if(owner.Debug){
            Debug.Log("--------------Goal List-------------");
            Goal currentGoal = ActiveGoal;
            GoalComposite currentCompositeGoal = ActiveGoal as GoalComposite;
            Debug.Log(currentGoal);
            while (currentCompositeGoal != null)
            {
                currentGoal = currentCompositeGoal.GetActiveGoal();
                if (currentGoal != null)
                {
                    Debug.Log(currentGoal);
                }
                currentCompositeGoal = currentGoal as GoalComposite;
            }
            /*Debug.Log("--------------Steering-------------");
            Debug.Log(owner.Steering.Behavior);
            Debug.Log(owner.transform.position);
            Debug.Log(owner.Steering.Destination);*/
        //}

        Goal_Evaluator mostDesirableGoal = Arbitrate();
        if (mostDesirableGoal == null){
            if(ActiveGoal != null) ActiveGoal.Process();
            return;
        }

        if(ActiveGoal == null || ActiveGoal.GetType() != mostDesirableGoal.GetGoalType()){
            if(ActiveGoal != null) ActiveGoal.Terminate();
            ActiveGoal = mostDesirableGoal.CreateGoal(owner);
        }

        if(ActiveGoal != null) ActiveGoal.Process();
    }

    private Goal_Evaluator Arbitrate () {
        Goal_Evaluator mostDesirableGoal = null;
        float bestDesirability = 0;

        foreach(Goal_Evaluator evaluator in evaluators){
            float desirability = evaluator.CalculateDesirability(owner, false);

            if(mostDesirableGoal == null || desirability > bestDesirability){
                mostDesirableGoal = evaluator;
                bestDesirability = desirability;
            }
        }

        if(ActiveGoal is GoalSync){
            GoalSync goalSync = ActiveGoal as GoalSync;
            if(goalSync.Evaluator == mostDesirableGoal || goalSync.Evaluator.CalculateDesirability(owner, false) >= bestDesirability){
                mostDesirableGoal = null;
            }
        }

        return mostDesirableGoal;
    }

    public bool RequestGoal (Goal_Evaluator goalRequested) {
        /*GoalSync goalSync = ActiveGoal as GoalSync;
        if(goalSync != null) return false;*/

        float goalRequestedDesirability = goalRequested.CalculateDesirability(owner, true);

        foreach(Goal_Evaluator evaluator in evaluators){
            float desirability = evaluator.CalculateDesirability(owner, false);
            if(evaluator == goalRequested) continue;

            if(desirability > goalRequestedDesirability) return false;
        }

        return true;
    }
}
