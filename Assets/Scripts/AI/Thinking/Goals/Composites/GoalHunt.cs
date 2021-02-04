using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GoalHunt : GoalComposite
{
    private enum HuntState {
        Search,
        SeekNest,
        Wander,
        Pursuit,
        Attack,
        Vacuum,
        Eat
    }

    public GoalHunt (Agent owner) : base(owner) {}

    private HuntState huntState = HuntState.Search;

    private float vacuumTime = 0;

    public override void Activate () {
        base.Activate();

        AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        Agent edible = null;
        float distanceToEdible = Mathf.Infinity;
        foreach(DataCreature data in creatures){
            if(!data.creature) continue;
            Agent agent = data.creature.agentCreature;

            if(data.RegistrationDate < Time.time - 2f || !agent.gameObject.activeSelf) continue;

            float distanceToAgent = Vector3.Distance(owner.transform.position, agent.transform.position);

            if(!ShouldAttack(agent.Creature)) continue;

            if(agent != owner && GetCreatureFilter()(agent.Creature) && distanceToAgent < distanceToEdible){
                edible = agent;
                distanceToEdible = distanceToAgent;
            }
        }

        switch (huntState){
            case HuntState.Search :
                    if(child.HasFailed) {
                        AddSubgoal(new GoalSeekNest(owner, new List<int>(owner.Memory.Species.GetByKey(owner.Creature.SpecieID).PreyIDs)));
                        huntState = HuntState.SeekNest;
                    }

                    if(!edible) break;

                    child.Abort();
                    AddSubgoal(new GoalPursuit(owner, edible));
                    //Ajout de son
                    //owner.Creature.AudioBox.PlayOneShot(SoundOneShot.CreatureFindFood);
                    huntState = HuntState.Pursuit;
                break;

            case HuntState.SeekNest :
                    if(child.HasFailed) {
                        AddSubgoal(new GoalWander(owner));
                        huntState = HuntState.Wander;
                    }

                    if(!edible) break;

                    child.Abort();
                    AddSubgoal(new GoalPursuit(owner, edible));
                    //Ajout de son
                    //owner.Creature.AudioBox.PlayOneShot(SoundOneShot.CreatureFindFood);
                    huntState = HuntState.Pursuit;

                break;

            case HuntState.Wander :
                    if(!edible) break;

                    child.Abort();
                    AddSubgoal(new GoalPursuit(owner, edible));
                    //Ajout de son
                    //owner.Creature.AudioBox.PlayOneShot(SoundOneShot.CreatureFindFood);
                    huntState = HuntState.Pursuit;

                break;
            
            case HuntState.Pursuit :
                if(child.IsComplete) {
                    if(Attacking.IsVictimWillDie(owner.Steering.Target.Creature, owner.Creature)){
                        AddSubgoal(new GoalEatCreature(owner, owner.Steering.Target.Creature));
                        huntState = HuntState.Eat;
                    }else{
                        AddSubgoal(new GoalAttack(owner, owner.Steering.Target.Creature));
                        huntState = HuntState.Attack;
                    }
                    
                }else if(child.HasFailed) {
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                    huntState = HuntState.Search;
                }else if(edible != null) {      
                    if(owner.Steering.Behavior == eSteeringBehavior.Pursuit && edible == owner.Steering.Target && !ShouldAttack(edible.Creature)) {
                        child.Abort();
                        AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                        huntState = HuntState.Search;
                    }else if(owner.Steering.Behavior == eSteeringBehavior.Seek || (owner.Steering.Behavior == eSteeringBehavior.Pursuit && edible != owner.Steering.Target)){
                        if((owner.Steering.Behavior == eSteeringBehavior.Seek &&
                             Vector3.Distance(owner.transform.position, edible.transform.position) + 5f < Vector3.Distance(owner.transform.position, owner.Steering.Destination))
                             || (owner.Steering.Behavior == eSteeringBehavior.Pursuit && (!owner.Steering.Target 
                             || Vector3.Distance(owner.transform.position, edible.transform.position) + 5f < Vector3.Distance(owner.transform.position, owner.Steering.Target.transform.position)))){
                            child.Abort();
                            AddSubgoal(new GoalPursuit(owner, edible));
                        }
                    }else if(owner.Creature.DNADistortion.HaveParticularity(typeof(Vacuum))){
                        if( Vector3.Distance(owner.transform.position, edible.transform.position) < 11) {
                            Vacuum vacuum = owner.Creature.DNADistortion.GetParticularity(typeof(Vacuum)) as Vacuum;

                            if(vacuum.IsRecharged()){
                                child.Abort();
                                AddSubgoal(new GoalLookAt(owner, edible));
                                huntState = HuntState.Vacuum;

                                vacuumTime = Time.time;
                                vacuum.Active();
                            }
                        }
                    }

                    /*if((owner.Steering.Behavior == eSteeringBehavior.Pursuit && edible != owner.Steering.Target)
                        || owner.Steering.Behavior == eSteeringBehavior.Seek && Vector3.Distance(owner.transform.position, edible.transform.position) + 5f < Vector3.Distance(owner.transform.position, owner.Steering.Destination)){
                        child.Abort();
                        AddSubgoal(new GoalPursuit(owner, edible));
                    }*/
                }else{
                    child.Abort();
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                    huntState = HuntState.Search;
                }
                break;

            case HuntState.Attack :
                if(child.IsComplete || child.HasFailed) {
                    AddSubgoal(new GoalPursuit(owner, owner.Steering.Target));
                    huntState = HuntState.Pursuit;
                }
                break;

            case HuntState.Vacuum :
                if(!edible){
                    status = GoalStatus.Failed;
                    break;
                }

                float distance = Vector3.Distance(owner.transform.position, edible.transform.position);
                if(distance < 3){
                    child.Abort();
                    AddSubgoal(new GoalEatCreature(owner, owner.Steering.Target.Creature));
                    huntState = HuntState.Eat;

                    (owner.Creature.DNADistortion.GetParticularity(typeof(Vacuum)) as Vacuum).Desactive();
                }else if(distance > 12 || Time.time - vacuumTime > 15){
                    (owner.Creature.DNADistortion.GetParticularity(typeof(Vacuum)) as Vacuum).Desactive();

                    status = GoalStatus.Failed;
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        if(owner.Creature.DNADistortion.HaveParticularity(typeof(Vacuum))){
            Vacuum vacuum = owner.Creature.DNADistortion.GetParticularity(typeof(Vacuum)) as Vacuum;
            if(vacuum.Actif) vacuum.Desactive();
        }

        base.Terminate();
    }

    private bool ShouldAttack (Creature creature){
        if(owner.Creature.Gauges.Hunger.Value < 100) return true;
        return ForceEvaluation.Ratio(owner, creature) >= 0.2f;
        //return true;
    }

    private Predicate<Creature> GetCreatureFilter () {
        return creature => {
            //if(creature.SpecieID == owner.Creature.SpecieID) return false;
            //return true;

            DataSpecies data = owner.Memory.Species.GetByKey(owner.Creature.SpecieID);
            if(data == null) return false;
            return data.PreyIDs.Contains(creature.SpecieID);
        };
    }
}
