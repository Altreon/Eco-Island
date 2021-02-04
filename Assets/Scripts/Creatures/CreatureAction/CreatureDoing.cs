using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
Classe : CreatureDoing
Classe qui permet à la créature d'accéder à ses différentes capacités
*/

public class CreatureDoing
{
    public Creature myCreature;

    public Action<Creature, Creature> CreatureAttack;
    public Action<Creature> CreatureAttackPlayer;
    public Action<Creature, Creature> CreatureEatCreature;
    public Action<Creature, Food> CreatureEatFood;
    public Action<Creature, Creature> CreatureBreed;
    public Action<Creature, Creature> CreatureShare;

    public CreatureDoing(Creature owner)
    {
        this.myCreature = owner;
    }
    public void Eat(Creature targetCreature, Action<bool> callback = null)
    {
        if (targetCreature == null) return;
        Eating.Instance.Eat(myCreature, targetCreature, callback);
        CreatureEatCreature?.Invoke(myCreature, targetCreature);

        if(myCreature.DNADistortion.HaveParticularity(typeof(Ghost))){
            (myCreature.DNADistortion.GetParticularity(typeof(Ghost)) as Ghost).Appear();
        }
    }

    public void EatFood(Food targetFood, Action<bool> callback = null)
    {
        if (targetFood == null) return;
        Eating.Instance.EatFood(myCreature, targetFood, callback);
        CreatureEatFood?.Invoke(myCreature, targetFood);

        if(myCreature.DNADistortion.HaveParticularity(typeof(Ghost))){
            (myCreature.DNADistortion.GetParticularity(typeof(Ghost)) as Ghost).Appear();
        }
    }

    public void Attack(Creature targetCreature)
    {
        if (targetCreature == null) return;
        Attacking.Instance.Attack(myCreature, targetCreature);
        CreatureAttack?.Invoke(myCreature, targetCreature);

        if(myCreature.DNADistortion.HaveParticularity(typeof(Ghost))){
            (myCreature.DNADistortion.GetParticularity(typeof(Ghost)) as Ghost).Appear();
        }
    }

    public void AttackPlayer ()
    {
        Attacking.Instance.AttackPlayer(myCreature);
        CreatureAttackPlayer?.Invoke(myCreature);

        if(myCreature.DNADistortion.HaveParticularity(typeof(Ghost))){
            (myCreature.DNADistortion.GetParticularity(typeof(Ghost)) as Ghost).Appear();
        }
    }

    public void Breed(Creature targetCreature)
    {
        if (targetCreature == null) return;
        Reproducing.Instance.Reproduce(myCreature, targetCreature);
        CreatureBreed?.Invoke(myCreature, targetCreature);

        if(myCreature.DNADistortion.HaveParticularity(typeof(Ghost))){
            (myCreature.DNADistortion.GetParticularity(typeof(Ghost)) as Ghost).Appear();
        }
    }

    private Discussion currentDiscussion = null;
    public Discussion CurrentDiscussion
    {
        get
        {
            return currentDiscussion;
        }
        set
        {
            currentDiscussion = value;
        }
    }

    public void Communicate(Creature targetCreature, MemoryType comType, Action callback = null)
    {
        if (targetCreature == null) return;
        Communicating.Instance.Communicate(myCreature, targetCreature, comType, callback);
        CreatureShare?.Invoke(myCreature, targetCreature);
    }

    public void StopCommunicate()
    {
        if (CurrentDiscussion != null) CurrentDiscussion.Stop();
    }
}
