using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalTalk : Goal
{
    private Agent partner;
    private MemoryType memoryType;
    private bool iAmLeader;


    private Action talkCallback;

    public GoalTalk(Agent owner, Agent partner, MemoryType memoryType, bool iAmLeader) : base(owner) {
        this.partner = partner;
        this.memoryType = memoryType;
        this.iAmLeader = iAmLeader;
    }

    public override void Activate () {
        base.Activate();

        if(!partner || !partner.gameObject.activeSelf){
            status = GoalStatus.Failed;
            return;
        }

        owner.Steering.Target = partner;
        owner.Steering.Behavior = eSteeringBehavior.LookAt;

        talkCallback += Talk;
        owner.Creature.CreatureDoing.Communicate(partner.Creature, memoryType, talkCallback);
    }

    public override void Process () {
        
    }

    private void Talk () {
        status = GoalStatus.Completed;
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        if(status != GoalStatus.Completed && iAmLeader){
            owner.Creature.CreatureDoing.StopCommunicate();
        }

        base.Terminate();
    }
}
