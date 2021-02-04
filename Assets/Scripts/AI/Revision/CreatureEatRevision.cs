using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureEatRevision : ActionRevision
{
    public void Revise(Agent reviseur, Creature from, Creature to) {
        DataSpecies data = reviseur.Memory.Species.GetByKey(from.SpecieID);
        if(data == null) return;

        data.addCarnivorousFood(new CarnivorousFood(to.SpecieID, Time.time));

        base.Revise(reviseur);
    }
}
