using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalAvoidPrey : Goal
{
    public GoalAvoidPrey (Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();
        
        owner.Steering.Behavior = eSteeringBehavior.Flee;
    }

    public override void Process () {
        List<int> preys = new List<int>(owner.Memory.Species.GetByKey(owner.Creature.SpecieID).CarnivorousFoods.Select(food => food.preyID));
        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        List<Agent> agentsToFlee = new List<Agent>();

        foreach(DataCreature data in creatures){
            Agent agent = data.creature?.agentCreature;
            if(!agent || !agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 5f) continue;

            bool isPrey = preys.Contains(agent.Creature.SpecieID);
            if(isPrey){
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
