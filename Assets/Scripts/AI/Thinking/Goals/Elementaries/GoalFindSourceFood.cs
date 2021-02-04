using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalFindSourceFood : Goal
{
    private List<SourceFood> visited;
    private SourceFood target;

    private Predicate<FoodType> filter;

    public GoalFindSourceFood(Agent owner, Predicate<FoodType> filter) : base(owner) {
        visited = new List<SourceFood>();
        this.filter = filter;
    }

    public override void Process () {
        IReadOnlyCollection<DataSourceFood> sources = owner.Memory.FoodSources.Read();
        DataSourceFood sourceFoodData = null;
        float distanceToSourceFood = Mathf.Infinity;
        foreach(DataSourceFood data in sources){
            if(!data.sourceFood) continue;
            SourceFood source = data.sourceFood;
            if(visited.Contains(source)) continue;

            if(!source.gameObject.activeSelf) continue;

            //ne pas prendre en compte les source food imédiatement vue.
            //if(Time.time - data.RegistrationDate > 1f) continue;

            if(!filter(source.FoodType)) continue;

            float distanceToSource = Vector3.Distance(owner.transform.position, source.transform.position);
            if(distanceToSource < distanceToSourceFood){
                sourceFoodData = data;
                distanceToSourceFood = distanceToSource;
            }
        }

        /*if(sourceFoodData != null){
            if(!owner.Steering.IsArrivedToDestination){
                owner.Steering.SetDestination(sourceFoodData.sourceFood.transform.position);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }else{
                if(owner.Steering.Behavior == eSteeringBehavior.Wander) return;

                owner.Steering.Behavior = eSteeringBehavior.Wander;
            }
        }else{
            status = GoalStatus.Failed;
        }*/

        if(sourceFoodData != null){
            if(target != sourceFoodData.sourceFood){
                target = sourceFoodData.sourceFood;
                owner.Steering.SetDestination(target.transform.position);
                owner.Steering.Behavior = eSteeringBehavior.Seek;
            }
            else if(owner.Steering.IsArrivedToDestination){
                visited.Add(target);
                target = null;
            }
            else if(target == null){
                status = GoalStatus.Failed;
            }
        }else{
            status = GoalStatus.Failed;
        }
    }

    public override void Terminate () {
        base.Terminate();
    }
}
