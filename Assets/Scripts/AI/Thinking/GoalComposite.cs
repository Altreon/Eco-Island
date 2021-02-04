using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : GoalComposite
Dérivé de But qui permet de créer et de gérer d'autre buts
*/

public abstract class GoalComposite : Goal
{
    private Queue<Goal> subgoals;

    public GoalComposite (Agent owner) : base(owner) {
        subgoals = new Queue<Goal>();
    }

    public override void Activate () {
        base.Activate();
    }

    public override void Terminate () {
        RemoveAllSubgoals();
        base.Terminate();
    }

    public void AddSubgoal (Goal goal) {
        subgoals.Enqueue(goal);
    }

    protected void ProcessSubgoals () {
        int nbSubgoals = subgoals.Count;

        bool lastGoalFailed = false;
        while(nbSubgoals != 0 &&    
              (subgoals.Peek().IsComplete ||
              subgoals.Peek().HasFailed)){
                  
            Goal endGoal = subgoals.Dequeue();
            lastGoalFailed = endGoal.HasFailed;
            endGoal.Terminate();
            --nbSubgoals;
        }

        if(nbSubgoals != 0) {
            Goal activeGoal = GetActiveGoal();
            if(activeGoal.IsInactive){
                activeGoal.Activate();
            }

            activeGoal.Process();

            if(activeGoal.IsComplete && nbSubgoals > 1){
                status = GoalStatus.Active;
            }
        }else{
            status = lastGoalFailed ? GoalStatus.Failed : GoalStatus.Completed;
        }
    }

    private void RemoveAllSubgoals () {
        foreach(Goal goal in subgoals){
            goal.Terminate();
        }

        subgoals.Clear();
    }

    public Goal GetActiveGoal () {
        if(subgoals.Count == 0) return null;
        return subgoals.Peek();
    }
}
