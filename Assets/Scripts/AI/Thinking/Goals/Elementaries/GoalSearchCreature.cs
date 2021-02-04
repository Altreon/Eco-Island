using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalSearchCreature : Goal
{

    private Predicate<Creature> filter;
    private List<Creature> seen;

    public GoalSearchCreature(Agent owner, Predicate<Creature> filter) : base(owner) {
        this.filter = filter;

        seen = new List<Creature>();
    }

    public override void Process () {
        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        DataCreature agentData = null;
        float distanceToAgent = Mathf.Infinity;
        foreach(DataCreature data in creatures){
            if(!data.creature) continue;
            Creature creature = data.creature;
            Agent agent = creature.agentCreature;
            Vector3 lastPos = data.lastPos;

            if(Time.time - data.RegistrationDate > 5f){
                seen.RemoveAll(c => c == creature);
            }

            if(agent == owner || !agent.gameObject.activeSelf) continue;
            if(seen.Contains(creature) || !filter(creature)) continue;

            float distanceToPos = Vector3.Distance(owner.transform.position, lastPos);
            if(agent != owner && distanceToPos < distanceToAgent){
                agentData = data;
                distanceToAgent = distanceToPos;
            }
        }

        if(agentData != null){
            if(Time.time - agentData.RegistrationDate < 0.5f){
                seen.Add(agentData.creature);
            }else if(!owner.Steering.IsArrivedToDestination){
                owner.Steering.SetDestination(agentData.lastPos);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }else{
                if(owner.Steering.Behavior == eSteeringBehavior.Wander) return;


                owner.Memory.Creatures.RemoveByKey(agentData.creature);
                owner.Steering.Behavior = eSteeringBehavior.Wander;
            }
        }else{
            /*if(owner.Steering.Behavior == eSteeringBehavior.Wander) return;

            owner.Steering.Behavior = eSteeringBehavior.Wander;*/
            status = GoalStatus.Failed;
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;
        base.Terminate();
    }
}
