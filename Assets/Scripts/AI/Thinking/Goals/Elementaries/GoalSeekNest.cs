using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSeekNest : Goal
{
    private List<int> speciesSeeked;

    private List<Nest> visited;
    private Nest target;

    public GoalSeekNest(Agent owner, List<int> speciesSeeked) : base(owner) {
        this.speciesSeeked = speciesSeeked;
        visited = new List<Nest>();
    }

    public GoalSeekNest(Agent owner) : base(owner) {
        speciesSeeked =  new List<int>();
        speciesSeeked.Add(owner.Creature.SpecieID);
        visited = new List<Nest>();
    }

    public override void Process () {
        IReadOnlyCollection<DataNest> nests = owner.Memory.Nests.Read();
        DataNest nestData = null;
        float distanceToNest = Mathf.Infinity;
        foreach(DataNest data in nests){
            if(!data.nest) continue;
            Nest nest = data.nest;
            if(visited.Contains(nest)) continue;

            if(!nest.gameObject.activeSelf) continue;

            if(!speciesSeeked.Contains(nest.SpecieID)) continue;

            //ne pas prendre en compte les nest food imédiatement vue.
            //if(Time.time - data.RegistrationDate > 1f) continue;

            float distanceToSource = Vector3.Distance(owner.transform.position, nest.transform.position);
            if(distanceToSource < distanceToNest){
                nestData = data;
                distanceToNest = distanceToSource;
            }
        }

        if(nestData != null){
            if(target != nestData.nest){
                target = nestData.nest;
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
