using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Memory
Module de mémoire, se charge de récupéré et de stocker les informations récupérées par l'agent
*/

public class Memory
{
    private Agent owner;

    //Temp Memory
    public MemoryObstacles Obstacles {get; private set;}
    public MemoryCreatures Creatures {get; private set;}
    public MemoryMeals Meals {get; private set;}
    public MemoryFoods Foods {get; private set;}
    public MemoryFoodSources FoodSources {get; private set;}
    public MemoryNests Nests {get; private set;}
    public MemoryCommunication Communications {get; private set;}

    public MemoryPlayer Player {get; private set;}

    //Persistant Memory
    public MemorySpecies Species{get; private set;}

    public Memory (Agent owner) {
        this.owner = owner;

        Obstacles = new MemoryObstacles();
        Creatures = new MemoryCreatures();
        Meals = new MemoryMeals();
        Foods = new MemoryFoods();
        FoodSources = new MemoryFoodSources();
        Nests = new MemoryNests();
        Communications = new MemoryCommunication();

        Player = new MemoryPlayer();

        Species = new MemorySpecies();
        Species.Write(new DataSpecies(owner.Creature.SpecieID));
    }

    public void Update () {
        Obstacles.Update();
        Creatures.Update();
        Meals.Update();
        Foods.Update();
        FoodSources.Update();
        Nests.Update();
        Communications.Update();
    }

    public void MergeFrom(Agent from, MemoryType type, float percent = 1){
        switch (type) {
            case MemoryType.Obstacles : Obstacles.MergeFrom(from.Memory.Obstacles, percent); break;
            case MemoryType.Creatures : Creatures.MergeFrom(from.Memory.Creatures, percent); break;
            case MemoryType.Meals : Meals.MergeFrom(from.Memory.Meals, percent); break;
            case MemoryType.Foods : Foods.MergeFrom(from.Memory.Foods, percent); break;
            case MemoryType.FoodSources : FoodSources.MergeFrom(from.Memory.FoodSources, percent); break;
            case MemoryType.Nests : Nests.MergeFrom(from.Memory.Nests, percent); break;
            case MemoryType.Communications : Communications.MergeFrom(from.Memory.Communications, percent); break;
            case MemoryType.Species : Species.MergeFrom(from.Memory.Species, percent); break;
        }
    }

}

public enum MemoryType {
    Obstacles,
    Creatures,
    Meals,
    Foods,
    FoodSources,
    Nests,
    Communications,
    Species
}
