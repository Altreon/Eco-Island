using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureSeeCreatureRevision : ActionRevision
{
    public void Revise(Agent reviseur, Creature saw) {
        if (reviseur.Creature.SpecieID == saw.SpecieID) return;
        if (reviseur.Memory.Species.ContainKey(saw.SpecieID)) return;

        reviseur.Memory.Species.Write(new DataSpecies(saw.SpecieID));

        base.Revise(reviseur);
    }
}
