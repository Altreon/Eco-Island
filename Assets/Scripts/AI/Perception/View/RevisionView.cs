using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
Classe : RevisionView
Permet d'identifier des situations particulières vues par l'agents, et de les transmettre aux modules de révision
*/

public class RevisionView
{
    private Agent owner;
    private List<Creature> OldList  = new List<Creature>();
    private List<Creature> NewList  = new List<Creature>();
    private List<Creature> AllSubscribedCreatures = new List<Creature>();

    public RevisionView(Agent owner)
    {
        this.owner = owner;
    }

    public void RevisionUpdate(List<Creature> OldList,List<Creature> NewList)
    {
        this.OldList = OldList;
        this.NewList = NewList;
        StartLoop();
    }

    private void StartLoop()
    {
        foreach(Creature creature in OldList)
        {
            if(!NewList.Contains(creature))
            {
                // La creature n'est plus dans la liste, on se désabonne
                AllSubscribedCreatures.Remove(creature);
                RemoveEvent(creature);
            }
        }

        foreach(Creature creature in NewList)
        {
            if(!OldList.Contains(creature))
            {
                Revision.Instance.ReviseCreatureSeeCreature(owner, creature);
                AllSubscribedCreatures.Add(creature);
                AttachEvent(creature);
            }
        }
    }

    private void AttachEvent(Creature creature)
    {
        creature.CreatureDie += DieCreatureEvent;
        creature.CreatureDoing.CreatureAttack += AttackCreatureEvent;
        creature.CreatureDoing.CreatureEatCreature += CreatureEatEvent;
        creature.CreatureDoing.CreatureEatFood += CreatureEatFoodEvent;
        creature.CreatureDoing.CreatureBreed += CreatureBreedEvent;
        creature.CreatureDoing.CreatureShare += CreatureShareEvent;
        creature.agentCreature.Steering.SteeringBehaviorChanged += CreatureSteeringEvent;
    }

    private void RemoveEvent(Creature creature)
    {
        creature.CreatureDie -= DieCreatureEvent;
        creature.CreatureDoing.CreatureAttack -= AttackCreatureEvent;
        creature.CreatureDoing.CreatureEatCreature -= CreatureEatEvent;
        creature.CreatureDoing.CreatureEatFood -= CreatureEatFoodEvent;
        creature.CreatureDoing.CreatureBreed -= CreatureBreedEvent;
        creature.CreatureDoing.CreatureShare -= CreatureShareEvent;
        creature.agentCreature.Steering.SteeringBehaviorChanged -= CreatureSteeringEvent;
    }

    public void RemoveAllEvent () 
    {
        foreach (Creature creature in AllSubscribedCreatures)
        {
            RemoveEvent(creature);
        }
    }

    private void CreatureSeeCreatureEvent(Creature creatureSeen)
    {

    }

    private void DieCreatureEvent(Creature creatureDie)
    {
        RemoveEvent(creatureDie);
        Revision.Instance.ReviseCreatureDead(owner,creatureDie);

        AllSubscribedCreatures.Remove(creatureDie);
        RemoveEvent(creatureDie);
    }

    private void AttackCreatureEvent(Creature from, Creature to)
    {
        Revision.Instance.ReviseCreatureAttack(owner,from,to);
    }

    private void CreatureEatEvent(Creature from, Creature to)
    {
        Revision.Instance.ReviseCreatureEatCreature(owner,from,to);
    }

    private void CreatureEatFoodEvent(Creature from, Food food)
    {
        float t = Time.time - Eating.timeToConsiderPlayerFood;
        if(food.LastPlayerHoldTime > t && from.agentCreature.Memory.Player.lastSeeTime > t){
            Revision.Instance.ReviseCreatureEatPlayerFood(owner, from, food.FoodType);
        }else{
            Revision.Instance.ReviseCreatureEatFood(owner,from,food.FoodType);
        }
    }

    private void CreatureBreedEvent(Creature partner1, Creature partner2)
    {
        Revision.Instance.ReviseCreatureReproduce(owner, partner1,partner2);
    }

    private void CreatureShareEvent(Creature partner1, Creature partner2)
    {
        Revision.Instance.ReviseCreatureCommunicate(owner, partner1,partner2);
    }

    private void CreatureSteeringEvent(Agent from, eSteeringBehavior Behavior)
    {
        switch (Behavior) {
            case eSteeringBehavior.Pursuit :
                Revision.Instance.ReviseCreaturePursuit(owner,from.Creature,from.Steering.Target.Creature);
                break;
            case eSteeringBehavior.PursuitPlayer :
                Revision.Instance.ReviseCreaturePursuitPlayer(owner, from.Creature);
                break;
            case eSteeringBehavior.FleePlayer :
            case eSteeringBehavior.HidePlayer :
                Revision.Instance.ReviseCreatureEvadePlayer(owner, from.Creature);
                break;
        }
    }
}
