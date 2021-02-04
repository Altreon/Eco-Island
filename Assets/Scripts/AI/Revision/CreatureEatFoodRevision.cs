using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureEatFoodRevision : ActionRevision
{
    public void Revise(Agent reviseur, Creature from, FoodType foodType) {
        DataSpecies data = reviseur.Memory.Species.GetByKey(from.SpecieID);
        if(data == null) return;

        data.addHerbivorFood(new HerbivorFood(foodType, Time.time));

        base.Revise(reviseur);
    }
}
