using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureAttackRevision : ActionRevision
{
    public void Revise(Agent reviseur, Creature from, Creature to) {
        DataSpecies data = reviseur.Memory.Species.GetByKey(from.SpecieID);
        if(data == null) return;

        bool fromIsPredateur = false;
        GoalComposite currentCompositeGoal = from.agentCreature.IsThinking ? from.agentCreature.Thinking.ActiveGoal as GoalComposite : null;
        while (currentCompositeGoal != null)
        {
            Goal currentGoal = currentCompositeGoal.GetActiveGoal();
            if (currentGoal.GetType() == typeof(GoalHunt))
            {
                fromIsPredateur = true;
                break;
            }
            currentCompositeGoal = currentGoal as GoalComposite;
        }

        if(fromIsPredateur) {
            data.addCarnivorousFood(new CarnivorousFood(to.SpecieID, Time.time));
        }

        base.Revise(reviseur);
    }
}
