using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureDeadRevision : ActionRevision
{
    public void Revise(Agent reviseur, Creature deadCreature) {
        reviseur.Memory.Creatures.RemoveByKey(deadCreature);

        Revise(reviseur);
    }
}
