using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreaturePursuitPlayerRevision : ActionRevision
{
    public void Revise(Agent reviseur, Creature from) {
        bool forAttack = from.agentCreature.Thinking.ActiveGoal is GoalDefensePlayer;

        if(forAttack && reviseur.Creature.SpecieID == from.SpecieID) {
            base.Revise(reviseur);
        }
    }
}
