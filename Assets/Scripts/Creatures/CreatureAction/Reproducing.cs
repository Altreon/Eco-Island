using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Reproducing
{
    private static Reproducing _instance;
    public static Reproducing Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Reproducing();
            }
            return _instance;
        }
    }

    [Min(0)]
    private int _asservissementValue = 2;
    public int AsservissementValue {
        get 
        {
            return _asservissementValue;
        }
        set
        {
            _asservissementValue = value;
        }
    }

    public void Reproduce(Creature parent1, Creature parent2)
    {
        if (parent1 == null || parent2 == null)
        {
            Debug.LogWarning("Attention, un des deux parents est vide");
            return;
        }

        parent1.Gauges.Reproduction.Value = parent1.Gauges.Reproduction.MaxSize;
        parent2.Gauges.Reproduction.Value = parent2.Gauges.Reproduction.MaxSize;

        CreatureTraits childTraits = GetChildTraits(parent1.Traits, parent2.Traits);

        // TODO
        // faudrais merge les listes d'alimentation des deux parent en vrai, là je prend juste le parent 1 pour implémenter le changement rapidement
        IReadOnlyCollection<CarnivorousFood> carnivorousFoodsParent1 = parent1.agentCreature.Memory.Species.GetByKey(parent1.SpecieID).CarnivorousFoods;
        IReadOnlyCollection<HerbivorFood> herbivorFoodsParent1 = parent1.agentCreature.Memory.Species.GetByKey(parent1.SpecieID).HerbivorFoods;

        List<int> carnivorousFoods = new List<int>(carnivorousFoodsParent1.Select(food => food.preyID));
        List<FoodType> herbivorFoods = new List<FoodType>(herbivorFoodsParent1.Select(food => food.foodType));

        List<ParticularityType> particularities = parent1.DNADistortion.AllParticularityTypes();

        Creature childCreature = CreatureFactory.Instance.GetCreature((parent1.transform.position + parent2.transform.position) / 2f + new Vector3(0, 1f, 0),
            Quaternion.Slerp(parent1.transform.rotation, parent2.transform.rotation, 0.5f),
            parent1.SpecieID,
            childTraits, null,
            carnivorousFoods, herbivorFoods,
            particularities,
            parent1.ColorSwap.GetColor());

        childCreature.agentCreature.Memory.MergeFrom(parent1.agentCreature, MemoryType.Species);
        childCreature.agentCreature.Memory.MergeFrom(parent2.agentCreature, MemoryType.Species);

        childCreature.agentCreature.Memory.MergeFrom(parent1.agentCreature, MemoryType.FoodSources);
        childCreature.agentCreature.Memory.MergeFrom(parent2.agentCreature, MemoryType.FoodSources);

        childCreature.agentCreature.Memory.MergeFrom(parent1.agentCreature, MemoryType.Nests);
        childCreature.agentCreature.Memory.MergeFrom(parent2.agentCreature, MemoryType.Nests);

        childCreature.Gauges.Hunger.Value = childCreature.Gauges.Hunger.MaxSize;
        childCreature.Gauges.Life.Value = childCreature.Gauges.Life.MaxSize;
        childCreature.Gauges.Reproduction.Value = childCreature.Gauges.Reproduction.MaxSize;

        if (parent2.transform.parent != null) childCreature.transform.parent = parent2.transform.parent;

        //Ajout de son
        childCreature.AudioBox.PlayOneShot(SoundOneShot.CreatureSpawn);

        childCreature.transform.GetChild(0).localScale = Vector3.one * 0.5f;

        ParticuleManager.Instance.CreateParticle(ParticleType.MagicExplosionBlue, childCreature.transform.position, Quaternion.identity);
    }

    private CreatureTraits GetChildTraits(CreatureTraits Traits1, CreatureTraits Traits2)
    {
        return new CreatureTraits(
            Traits1.Aggressivity, // Pas d'asservissement
            Traits1.Carnivorous, //Pas d'asservissement
            Traits1.Constitution,  //Pas d'asservissement
            Traits1.Gluttony, //Pas d'asservissement
            Traits1.Lust,  //Pas d'asservissement
            Traits1.Hysteresis,  //Pas d'asservissement
            Traits1.Sociability,  //Pas d'asservissement
            Traits1.Curiosity,  //Pas d'asservissement
            Traits1.Strength, //Pas d'asservissement
            Traits1.Vision, //Pas d'asservissement
            Traits1.Speed, //Pas d'asservissement
            LoveFormula(Traits1.Vigilance, Traits2.Vigilance, 0.3f),
            Traits1.Nocturnal //Pas d'asservissement
            );
    }

    private float LoveFormula(float trait1, float trait2, float balance = 0.5f)
    {
        float traitm = (trait1 + trait2) / 2;
        float asserv = (traitm - balance) / _asservissementValue;
        return traitm - asserv;
    }
}
