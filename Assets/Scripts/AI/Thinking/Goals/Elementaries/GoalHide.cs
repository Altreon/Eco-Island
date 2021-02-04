using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHide : Goal
{
    public GoalHide (Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();

        owner.Steering.Behavior = eSteeringBehavior.Hide;
    }

    public override void Process () {
        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        List<Agent> agentsToFlee = new List<Agent>();

        foreach(DataCreature data in creatures){
            Agent agent = data.creature?.agentCreature;
            if(!agent || !agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 5f) continue;

            bool isHostil = GoalEvade.CreatureIsHostil(owner, agent.Creature);
            if(isHostil){
                agentsToFlee.Add(agent);
            }
        }

        owner.Steering.Evades = agentsToFlee;

        if(agentsToFlee.Count == 0){
            status = GoalStatus.Completed;
        }
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }
}
