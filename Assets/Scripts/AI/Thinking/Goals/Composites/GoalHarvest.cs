using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GoalHarvest : GoalComposite
{
    private enum HarvestState {
        Search,
        Catch,
        Eat
    }

    public GoalHarvest (Agent owner) : base(owner) {}

    private HarvestState harvestState = HarvestState.Search;

    public override void Activate () {
        base.Activate();

        AddSubgoal(new GoalSearchFood(owner, GetFoodTypeFilter()));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        IReadOnlyCollection<DataFood> foods = owner.Memory.Foods.Read();
        Food edible = null;
        float distanceToEdible = Mathf.Infinity;
        foreach(DataFood data in foods){
            if(!data.food) continue;
            Food food = data.food;
            if(!food || !food.gameObject.activeSelf || !food.Eatable) continue;
            if(Player.Instance.PlayerPickAndDrop.IsHandleFood && Player.Instance.PlayerPickAndDrop.TransportableFruit == food) continue;

            if(data.RegistrationDate < Time.time - 0.5f) continue;

            if(!GetFoodTypeFilter()(food.tag == "Fruit" ? FoodType.Fruit : FoodType.Vegetable)) continue;

            float distanceToFood = Vector3.Distance(owner.transform.position, food.transform.position);
            if(distanceToFood < distanceToEdible){
                edible = food;
                distanceToEdible = distanceToFood;
            }
        }

        switch (harvestState){
            case HarvestState.Search :
                    if(!edible) break;

                    child.Abort();
                    AddSubgoal(new GoalCatch(owner, edible.transform));
                    //Ajout de son
                    //owner.Creature.AudioBox.PlayOneShot(SoundOneShot.CreatureFindFood);
                    harvestState = HarvestState.Catch;
                break;
            
            case HarvestState.Catch :
                if(child.IsComplete) {
                    AddSubgoal(new GoalEatFood(owner, owner.Steering.Aim));
                    harvestState =  HarvestState.Eat;
                }else if(child.HasFailed) {
                    AddSubgoal(new GoalSearchFood(owner, GetFoodTypeFilter()));
                    harvestState =  HarvestState.Search;
                }else if(edible != null){

                    if((owner.Steering.Behavior == eSteeringBehavior.Catch && edible != owner.Steering.Aim.gameObject)
                        || owner.Steering.Behavior == eSteeringBehavior.Seek && Vector3.Distance(owner.transform.position, edible.transform.position) < Vector3.Distance(owner.transform.position, owner.Steering.Destination)){
                        AddSubgoal(new GoalCatch(owner, edible.transform));
                        child.Abort();
                    }
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }

    private Predicate<FoodType> GetFoodTypeFilter () {
        return foodType => {
            DataSpecies data = owner.Memory.Species.GetByKey(owner.Creature.SpecieID);
            if(data == null) return false;
            return data.EatFoodType.Contains(foodType);
        };
    }
}
