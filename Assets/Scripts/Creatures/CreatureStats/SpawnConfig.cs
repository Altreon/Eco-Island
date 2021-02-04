using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Specie
{
    public int NbCreaturesInit = 10;
    public bool randomizeOneTraitOnly;
    public CREATURE_TRAITS randomizedTraitOnly;
    [Range(0f, 1f)]
    public float randomizer;
    public CreatureTraits specieTraits;
    public List<int> CarnivorousFoods;
    public List<FoodType> HerbivorFoods;
    public List<ParticularityType> particularities;
    public Color defaultColor;
}
[CreateAssetMenu(fileName = "SpawnConfig", menuName = "Creatures/Spawn Config", order = 6)]
public class SpawnConfig : ScriptableObject
{
    public Specie[] Species;
    //[Min(0)]
    //public int NbCreaturesPerSpecie = 0;
}
