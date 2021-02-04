using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalSearchFood : GoalComposite
{
    private enum SearchFoodState {
        Find,
        Communicate,
        Wander,
    }

    private Predicate<FoodType> filter;

    public GoalSearchFood (Agent owner, Predicate<FoodType> filter) : base(owner) {
        this.filter = filter;
    }

    private SearchFoodState searchFoodState = SearchFoodState.Find;

    public override void Activate () {
        base.Activate();
        AddSubgoal(new GoalFindSourceFood(owner, filter));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        switch (searchFoodState){
            case SearchFoodState.Find :
                if(child.HasFailed) {
                    //TODO
                    AddSubgoal(new GoalCommunicate(owner, MemoryType.FoodSources));
                    searchFoodState = SearchFoodState.Communicate;
                    //AddSubgoal(new GoalWander(owner));
                    //searchFoodState = SearchFoodState.Wander;
                }
                break;

            case SearchFoodState.Communicate :
                if(child.IsComplete || child.HasFailed){
                    AddSubgoal(new GoalFindSourceFood(owner, filter));
                    searchFoodState = SearchFoodState.Find;
                }
                break;
            
            case SearchFoodState.Wander :
                
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }

    /*public GoalSearchFood(Agent owner) : base(owner) {}

    public override void Process () {
        IReadOnlyCollection<DataSourceFood> sources = owner.Memory.FoodSources.Read();
        DataSourceFood sourceFoodData = null;
        float distanceToSourceFood = Mathf.Infinity;
        foreach(DataSourceFood data in sources){
            if(!data.sourceFood) continue;
            SourceFood source = data.sourceFood;

            if(!source.gameObject.activeSelf) continue;

            float distanceToSource = Vector3.Distance(owner.transform.position, source.transform.position);
            if(distanceToSource < distanceToSourceFood){
                sourceFoodData = data;
                distanceToSourceFood = distanceToSource;
            }
        }

        if(sourceFoodData != null){
            if(!owner.Steering.IsArrivedToDestination){
                owner.Steering.SetDestination(sourceFoodData.sourceFood.transform.position);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }else{
                if(owner.Steering.Behavior == eSteeringBehavior.Wander) return;

                owner.Memory.FoodSources.Remove(sourceFoodData);
                owner.Steering.Behavior = eSteeringBehavior.Wander;
            }
        }else{
            if(owner.Steering.Behavior == eSteeringBehavior.Wander) return;
            
            owner.Steering.Behavior = eSteeringBehavior.Wander;
        }

        IReadOnlyCollection<DataSourceFood> sources = owner.Memory.FoodSources.Read();
        DataSourceFood sourceFoodData = null;
        float distanceToSourceFood = Mathf.Infinity;
        foreach(DataSourceFood data in sources){
            if(!data.sourceFood) continue;
            SourceFood source = data.sourceFood;

            if(!source.gameObject.activeSelf) continue;

            float distanceToSource = Vector3.Distance(owner.transform.position, source.transform.position);
            if(distanceToSource < distanceToSourceFood){
                sourceFoodData = data;
                distanceToSourceFood = distanceToSource;
            }
        }

        if(sourceFoodData != null){
            if(!owner.Steering.IsArrivedToDestination){
                owner.Steering.SetDestination(sourceFoodData.sourceFood.transform.position);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }else{
                owner.Memory.FoodSources.Remove(sourceFoodData);

                status = GoalStatus.Failed;
            }
        }else{
            status = GoalStatus.Failed;
        }

        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        Creature closestAlly = null;
        float distanceToAlly = Mathf.Infinity;
        foreach(DataCreature data in creatures){
            if(!data.creature) continue;
            Creature creature = data.creature;

            if(!creature.gameObject.activeSelf) continue;

            float distance = Vector3.Distance(owner.transform.position, creature.transform.position);
            if(distance < distanceToAlly){
                closestAlly = creature;
                distanceToAlly = distance;
            }
        }

        if(closestAlly != null){
            if(!owner.Steering.IsArrivedToDestination){
                owner.Steering.SetDestination(sourceFoodData.sourceFood.transform.position);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }else{
                if(owner.Steering.Behavior == eSteeringBehavior.Wander) return;

                owner.Memory.FoodSources.Remove(sourceFoodData);
                owner.Steering.Behavior = eSteeringBehavior.Wander;
            }
            return;
        }
    }

    public override void Terminate () {
        base.Terminate();
    }*/
}
