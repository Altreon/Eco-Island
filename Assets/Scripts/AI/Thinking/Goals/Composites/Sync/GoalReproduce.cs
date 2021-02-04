using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalReproduce : GoalSync
{
    private enum ReproduceState {
        Pursuit,
        Breed
    }

    private ReproduceState reproduceState = ReproduceState.Pursuit;

    private Agent partner;
    private MemoryType memoryType;
    private bool iAmleader;

    private Squad squad;

    private Action<bool> reproduceCallback;

    public GoalReproduce(Agent owner, Agent partner, Squad squad) : base(owner) {
        this.partner = partner;
        this.squad = squad;

        iAmleader = squad.Leader == owner;
    }

    public override void Activate () {
        base.Activate();

        if(squad.IsDisband){
            status = GoalStatus.Failed;
            return;
        }

        AddSubgoal(new GoalPursuit(owner, partner));
    }

    public override void Process () {
        if(status == GoalStatus.Inactive) Activate();
        if(status == GoalStatus.Failed) return;

        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        if(squad.IsDisband){
            status = GoalStatus.Failed;
        }else{
            //squad.DebugLine();
        }

        switch (reproduceState){
            case ReproduceState.Pursuit :
                if(child.IsComplete) {
                    if(iAmleader){
                        AddSubgoal(new GoalBreed(owner, partner.Creature));
                        reproduceState = ReproduceState.Breed;
                    }else{
                        AddSubgoal(new GoalIdle(owner));
                        reproduceState = ReproduceState.Breed;
                    }
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override Goal_Evaluator Evaluator {
        get {
            return GoalReproduction_Evaluator.Instance;
        }
    }

    public override void Terminate () {
        if(!squad.IsDisband){
            squad.Remove(owner);
        }

        base.Terminate();
    }
}
