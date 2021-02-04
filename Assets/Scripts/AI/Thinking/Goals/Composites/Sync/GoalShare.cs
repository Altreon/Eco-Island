using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalShare : GoalSync
{
    private enum ShareState {
        Pursuit,
        Talk
    }

    private ShareState shareState = ShareState.Pursuit;

    private Agent partner;
    private MemoryType memoryType;
    private bool iAmLeader;

    private Squad squad;

    private Action<bool> shareCallback;

    /*public GoalShare(Agent owner, Agent partner, MemoryType memoryType, bool iAmleader) : base(owner) {
        this.partner = partner;
        this.memoryType = memoryType;
    }

    public override void Activate () {
        base.Activate();

        if(!target || !target.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        shareCallback += Share;
        owner.Creature.CreatureDoing.Communicate(target.Creature, memoryType, shareCallback);
    }*/

    public GoalShare(Agent owner, Agent partner, MemoryType memoryType, Squad squad) : base(owner) {
        this.partner = partner;
        this.memoryType = memoryType;
        this.squad = squad;

        iAmLeader = squad.Leader == owner;
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

        

        switch (shareState){
            case ShareState.Pursuit :
                if(child.IsComplete) {
                    if(iAmLeader){
                        AddSubgoal(new GoalTalk(owner, partner, memoryType, iAmLeader));
                        shareState = ShareState.Talk;
                    }else{
                        AddSubgoal(new GoalLookAt(owner, partner));
                        shareState = ShareState.Talk;
                    }
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override Goal_Evaluator Evaluator {
        get {
            return GoalCommunication_Evaluator.Instance;
        }
    }

    public override void Terminate () {
        if(!squad.IsDisband){
            squad.Remove(owner);
        }

        base.Terminate();
    }
}
