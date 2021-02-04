using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageDoing : MonoBehaviour
{
    public Creature activeCreature;

    public void Eat()
    {
        activeCreature.CreatureDoing.Eat(activeCreature.targetCreature);
    }

    public void Attack()
    {
        activeCreature.CreatureDoing.Attack(activeCreature.targetCreature);
    }

    public void Breed()
    {
        activeCreature.CreatureDoing.Breed(activeCreature.targetCreature);
    }

    public void Share()
    {
        activeCreature.CreatureDoing.Communicate(activeCreature.targetCreature, MemoryType.Creatures);
    }
}
