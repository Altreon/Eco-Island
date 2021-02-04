using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

/**
Classe : CreatureFactory
Factory prenant en charge la création des créatures dans le monde du jeu
*/

/*public class CreatureReferences
{
    public CreatureReferences(GameObject instantiatedGo = null, Creature valuresRef = null, Action<Trait> listener = null)
    {
        go = instantiatedGo;
        values = valuresRef;
        listenerEvent = listener;
    }
    public GameObject go;
    public Creature values;
    public Action<Trait> listenerEvent;
}*/

public class CreatureFactory : MonoBehaviour
{
    #region SINGLETON

    
    private static CreatureFactory _instance = null;
    public static CreatureFactory Instance => _instance;

    public bool dontDestroyOnLoad = false;
    public bool customColor = true;
    
    void Awake()
    {
        if (_instance == null && enabled)
        {
            _instance = this;
            if(dontDestroyOnLoad)
                DontDestroyOnLoad(this.gameObject);
            //spawnedCreatures = new Dictionary<GameObject, CreatureReferences>();
            aliveCreatures = new List<Creature>();
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    //[System.NonSerialized] private Dictionary<CREATURE_GAUGES, Tuple<float, float>> boundsRandomRateGauges;
    private List<Creature> aliveCreatures;

    public IReadOnlyList<Creature> AliveCreature {get{ return aliveCreatures.AsReadOnly(); }}
    
    [SerializeField] private Creature CreaturePrefab = null;
   
    [SerializeField] private GaugesConfig configGauges = null;
    [SerializeField] public SpawnConfig configSpawn;


    //compte créature
    private int[] _creatureCountTable;
    public int[] CreatureCountTable {
        get
        {
            return _creatureCountTable;
        }
        private set
        {
            _creatureCountTable = value;
        }
    }
    
    // Arduino Event
    public Action CreatureBirth;
    public Action DeadCreature;
    public Action MealCreature;

    //TODO temps à changer
    List<Nest> nests;

    public void Start()
    {
        Assert.IsNotNull(CreaturePrefab);
        Assert.IsNotNull(configGauges);
        Assert.IsNotNull(configSpawn);
        /*
        boundsRandomRateGauges = new Dictionary<CREATURE_GAUGES, Tuple<float, float>>();
        foreach (GaugeOperation gaugeOperation in configGauges.modifiers)
        {
            if (gaugeOperation.valueChanges == GAUGE_VALUE.RATE)
            {
                boundsRandomRateGauges.Add(gaugeOperation.influenced, new Tuple<float, float>(gaugeOperation.value, gaugeOperation.traitInfluence));
            }
        }
        */
    }

    private bool spawnisSetup = false;
    
    public List<Creature>[] SetupSpawn()
    {
        //J'ai écrit cette fonction à 1h30 ne me jugez pas :sob:
        //Assert.IsTrue(nbTotalIndividuals > 0);
        if (spawnisSetup)
        {
            Debug.LogWarning("Are you sure about that ? Hasn't the setup already been made ?");
        }

        spawnisSetup = true;
        
        //int nbIndividualsPerSpecie = configSpawn.NbCreaturesPerSpecie;
        //int nbMoreOfFirstSpecie = nbTotalIndividuals % configSpawn.Species.Length;
        List<Creature>[] spawnedCreatures = new List<Creature>[configSpawn.Species.Length];
        for (int i = 0; i < configSpawn.Species.Length; ++i)
        {
            spawnedCreatures[i] = new List<Creature>();
        }
        /*CreatureTraits newCreatureTraitFIRST = null;
        for (int i = 0; i < nbMoreOfFirstSpecie; ++i)
        {
            if (i == 0)
            {
                newCreatureTraitFIRST = new CreatureTraits(configSpawn.Species[0].specieTraits);
                if (configSpawn.Species[0].randomizeOneTraitOnly)
                {
                    newCreatureTraitFIRST.Get(configSpawn.Species[0].randomizedTraitOnly).AddValueClamped(UnityEngine.Random.Range(
                        (newCreatureTraitFIRST.Get(configSpawn.Species[0].randomizedTraitOnly).Value - configSpawn.Species[0].randomizer) < 0f ? 0f :
                            configSpawn.Species[0].randomizer * -1f,
                        (newCreatureTraitFIRST.Get(configSpawn.Species[0].randomizedTraitOnly).Value + configSpawn.Species[0].randomizer) > 1f ? 1f :
                            configSpawn.Species[0].randomizer));
                }
                else
                {
                    foreach (CREATURE_TRAITS traits in (CREATURE_TRAITS[])System.Enum.GetValues(typeof(CREATURE_TRAITS)))
                    {
                        newCreatureTraitFIRST.Get(traits)
                            .AddValueClamped(UnityEngine.Random.Range(
                                (newCreatureTraitFIRST.Get(traits).Value - configSpawn.Species[0].randomizer) < 0f ? 0f :
                                    configSpawn.Species[0].randomizer * -1f,
                                (newCreatureTraitFIRST.Get(traits).Value + configSpawn.Species[0].randomizer) > 1f ? 1f :
                                    configSpawn.Species[0].randomizer));
                    }
                }
            }
            
            //TODO FORMULE POUR ATTACK POWER, NUTRITIONNAL VALUE, MAX LIFE NUMBER
            int attackPow = (int) configSpawn.Species[0].specieTraits.Strength * 15 + 5;
            int nuttritionnalValue = (int) configSpawn.Species[0].specieTraits.Constitution * 100 + 50;
            int maxLifeNumber = 50;
            GameObject spawnedCreature = GetCreature(null, Vector3.zero, Quaternion.identity, attackPow,
                nuttritionnalValue, maxLifeNumber, 1, newCreatureTraitFIRST, null);
            spawnedCreature.SetActive(false);
            spawnedCreatures[0][i] = spawnedCreature;
        }*/

        //Pour le compte d'individu
        CreatureCountTable = new int[configSpawn.Species.Length];
        //
        int specieID = 0;
        foreach (Specie specie in configSpawn.Species)
        {
            int nbCreatures = configSpawn.Species[specieID].NbCreaturesInit;
            List<Creature> creatures = CreateCreaturesFromConfig (specieID, nbCreatures);

            spawnedCreatures[specieID].AddRange(creatures);

            ++specieID;
        }

        return spawnedCreatures;
    }

    private List<Creature> CreateCreaturesFromConfig (int specieID, int nb) {
        List<Creature> creatures = new List<Creature>();

        Specie specie = configSpawn.Species[specieID];
        float boundRandMin = specie.randomizer * -1f;
        float boundRandMax = specie.randomizer;
        //TODO FORMULE POUR ATTACK POWER, NUTRITIONNAL VALUE, MAX LIFE NUMBER
        //TODO je le fais dans l'init physique de l'agent
        /*int attackPow = (int) specie.specieTraits.Strength * 15 + 5;
        int nuttritionnalValue = (int) specie.specieTraits.Constitution * 100 + 50;*/
        //int maxLifeNumber = 50;
        CreatureTraits newCreatureTrait = new CreatureTraits(specie.specieTraits);
        if (specie.randomizeOneTraitOnly)
        {
            float traitValue = specie.specieTraits.Get(specie.randomizedTraitOnly).Value;
            newCreatureTrait.Get(specie.randomizedTraitOnly).AddValueClamped(UnityEngine.Random.Range(
                    (traitValue + boundRandMin) < 0f ? 0f : boundRandMin,
                    (traitValue + boundRandMax) > 1f ? 1f : boundRandMax
                )
            );
        }
        else
        {
            foreach (CREATURE_TRAITS traits in (CREATURE_TRAITS[])System.Enum.GetValues(typeof(CREATURE_TRAITS)))
            {
                float traitValue = specie.specieTraits.Get(traits).Value;
                newCreatureTrait.Get(traits).AddValueClamped(UnityEngine.Random.Range(
                        (traitValue + boundRandMin) < 0f ? 0f : boundRandMin,
                        (traitValue + boundRandMax) > 1f ? 1f : boundRandMax
                    )
                );
            }
        }
        for (int i = 0 ; i < nb; ++i)
        {
            Creature spawnedCreature = GetCreature(Vector3.zero, Quaternion.identity,
                specieID, newCreatureTrait, null, specie.CarnivorousFoods, specie.HerbivorFoods,
                specie.particularities, specie.defaultColor);
            spawnedCreature.gameObject.SetActive(false);

            spawnedCreature.Age = UnityEngine.Random.Range(0, 0.5f);

            for (int k = 0 ; k < configSpawn.Species.Length; ++k) {
                Specie specieInfo = configSpawn.Species[k];
                DataSpecies data = new DataSpecies(k);
                foreach(int food in specieInfo.CarnivorousFoods){
                    data.addCarnivorousFood(new CarnivorousFood(food, Time.time));
                }
                foreach(FoodType food in specieInfo.HerbivorFoods){
                    data.addHerbivorFood(new HerbivorFood(food, Time.time));
                }
                spawnedCreature.agentCreature.Memory.Species.Write(data);
            }

            creatures.Add(spawnedCreature);
        }

        return creatures;
    }

    public void StartGame()
    {   
        nests = new List<Nest>(GameObject.FindGameObjectsWithTag("Nest").Select(obj => obj.GetComponent<Nest>()));

        foreach (Creature spawnedCreature in aliveCreatures)
        {
            spawnedCreature.gameObject.SetActive(true);

            Nest nest = nests != null ? nests.FirstOrDefault(n => n.SpecieID == spawnedCreature.SpecieID) : null;
            if(nest){
                spawnedCreature.agentCreature.Memory.Nests.Write(new DataNest(nest));
            }
        }
    }

    /*public void Reproduce(GameObject parent1, GameObject parent2)
    {
        Assert.IsNotNull(parent1);
        Assert.IsNotNull(parent2);
        GetCreature(parent1.transform.parent, (parent1.transform.position + parent2.transform.position) / 2f + new Vector3(0, 1f, 0),
            Quaternion.Slerp(parent1.transform.rotation, parent2.transform.rotation, 0.5f),
            spawnedCreatures[parent1].values.attackPower, spawnedCreatures[parent1].values.nutritionnal,
            spawnedCreatures[parent1].values.maxLifeNumber, spawnedCreatures[parent1].values.SpecieID,
            spawnedCreatures[parent1].values.Traits, null);
        //ARDUINO ISLAND
        parent1.GetComponent<Creature>();
        //
    }*/

    /*public Creature Reproduce(Creature parent1, Creature parent2)
    {
        Assert.IsNotNull(parent1);
        Assert.IsNotNull(parent2);

        // TODO
        // faudrais merge les listes d'alimentation des deux parent en vrai, là je prend juste le parent 1 pour implémenter le changement rapidement
        IReadOnlyCollection<CarnivorousFood> carnivorousFoodsParent1 = parent1.agentCreature.Memory.Species.GetByKey(parent1.SpecieID).CarnivorousFoods;
        IReadOnlyCollection<HerbivorFood> herbivorFoodsParent1 = parent1.agentCreature.Memory.Species.GetByKey(parent1.SpecieID).HerbivorFoods;

        List<int> carnivorousFoods = new List<int>(carnivorousFoodsParent1.Select(food => food.preyID));
        List<FoodType> herbivorFoods = new List<FoodType>(herbivorFoodsParent1.Select(food => food.foodType));

        Creature creature = GetCreature((parent1.transform.position + parent2.transform.position) / 2f + new Vector3(0, 1f, 0),
            Quaternion.Slerp(parent1.transform.rotation, parent2.transform.rotation, 0.5f),
            parent1.SpecieID,
            parent1.Traits, null,
            carnivorousFoods, herbivorFoods);

        creature.agentCreature.Memory.MergeFrom(parent1.agentCreature, MemoryType.Species);
        creature.agentCreature.Memory.MergeFrom(parent2.agentCreature, MemoryType.Species);

        return creature;
    }*/

    /*public GameObject[] AllSpawnedGameObjects
    {
        get
        {
            return spawnedCreatures.Keys.ToArray();
        }
    }*/

    /*public Creature[] AllSpawnedCreatures
    {
        get
        {
            Creature[] returned = new Creature[spawnedCreatures.Count];
            int i = -1;
            foreach (CreatureReferences value in spawnedCreatures.Values)
            {
                ++i;
                returned[i] = value.values;
            }

            return returned;
        }
    }*/
    //private Dictionary<GameObject, CreatureReferences> spawnedCreatures;

    /*public Creature GetCreature(Transform newParent, Vector3 worldPosition, Quaternion rotation, int attackPower, int nutritionnal, int maxLifeNumber, int SpecieID, CreatureTraits newTrait, CreatureGauges newGauge)
    {
        CreatureReferences refCreat = new CreatureReferences();
        Creature newCreature = Instantiate(CreaturePrefab, worldPosition, rotation);

        if(newParent != null)
            newCreature.transform.SetParent(newParent);
        newCreature.transform.position = worldPosition;
        newCreature.transform.rotation = rotation;
        newCreature.nutritionnal = nutritionnal;
        newCreature.attackPower = attackPower;
        newCreature.maxLifeNumber = maxLifeNumber;
        newCreature.SpecieID = SpecieID;

        if(newTrait != null)
            newCreature.Traits = new CreatureTraits(newTrait);
        if(newGauge != null)
            newCreature.Gauges = new CreatureGauges(newGauge);

        newCreature.gameObject.SetActive(true);

        configGauges.InitializeGauges(newCreature, true, false, false);

        refCreat.go = newCreature.gameObject;
        refCreat.values = newCreature;
        refCreat.listenerEvent = (x) => updateGaugesOnTraitModified(x, refCreat.values);
        refCreat.values.Traits.UpdatedTrait += refCreat.listenerEvent;
        spawnedCreatures[newCreature] = refCreat;
        refCreat.values.ColorSwap.Swap(CreatureTraits.GetColor(refCreat.values.Traits));
        //Metrics.NotifyCreatureCreated(refCreat.values);
        return newCreature;
    }*/

    public Creature GetCreature(Vector3 worldPosition, Quaternion rotation, int SpecieID, 
                                CreatureTraits newTrait, CreatureGauges newGauge, 
                                List<int> carnivorousFoods, List<FoodType> herbivorFoods,
                                List<ParticularityType> particularities, Color color)
    { 
        Creature newCreature = Instantiate(CreaturePrefab, worldPosition, rotation);

        //newCreature.attackPower = attackPower;
        //newCreature.maxLifeNumber = maxLifeNumber;
        newCreature.SpecieID = SpecieID;

        if (newTrait != null)
            newCreature.Traits = new CreatureTraits(newTrait);
        if (newGauge != null) 
            newCreature.Gauges = new CreatureGauges(newGauge);

        foreach(ParticularityType particularityType in particularities){
            newCreature.DNADistortion.AddParticularity(ParticularitiesManager.Instance.CreateParticularity(newCreature, particularityType));
        }

        newCreature.agentCreature.Init();

        DataSpecies data = newCreature.agentCreature.Memory.Species.GetByKey(SpecieID);
        if(newTrait.Carnivorous > 0.5f){
            foreach(int food in carnivorousFoods){
                if(food == SpecieID) continue;
                data.addCarnivorousFood(new CarnivorousFood(food, Time.time));
            }
        }else{
            foreach(FoodType food in herbivorFoods){
                data.addHerbivorFood(new HerbivorFood(food, Time.time));
            }
        }

        Nest nest = nests != null ? nests.FirstOrDefault(n => n.SpecieID == SpecieID) : null;
        if(nest){
            newCreature.agentCreature.Memory.Nests.Write(new DataNest(nest));
        }

        newCreature.gameObject.SetActive(true);

        configGauges.InitializeGauges(newCreature, true, false, false);

        if(customColor){
            newCreature.ColorSwap.Swap(color);
        }else{
            newCreature.ColorSwap.Swap(CreatureTraits.GetColor(newCreature.Traits));
        }

        aliveCreatures.Add(newCreature);
        //ARDUINO ISLAND
        if (CreatureCountTable != null) ++CreatureCountTable[newCreature.SpecieID];

        // Jirachi
        creaturePopulationChangedEvent?.Invoke();
        
        // Lucas
        CreatureBirth?.Invoke();
        newCreature.CreatureDoing.CreatureEatCreature += CreatureEatCreature;
        newCreature.CreatureDoing.CreatureEatFood += CreatureEatFood;
        /*Debug.Log("--------------CreatureList-------------");
        foreach (int specie in CreatureCountTable)
        {
            Debug.Log(specie);
        }
        Debug.Log("-----------");*/
        //

        //Metrics.NotifyCreatureCreated(refCreat.values);
        return newCreature;
    }

    /*private void updateGaugesOnTraitModified(Trait updated, Creature changed)
    {
        configGauges.InitializeGauges(changed, false, true, false);
        changed.ColorSwap.Swap(CreatureTraits.GetColor(changed.Traits));
    }*/

    public void DestroyCreature(Creature toDestroy)
    {
        int specieID = toDestroy.SpecieID;

        //Metrics.NotifyCreatureDied(refToDestroy.values);
        aliveCreatures.Remove(toDestroy);
        //ARDUINO ISLAND
        if (CreatureCountTable != null) --CreatureCountTable[toDestroy.SpecieID];

        // Jirachi
        creaturePopulationChangedEvent?.Invoke();

        // Lucas
        DeadCreature?.Invoke();
        //TODO je me suis permis de commenter le debug parce que ça fait longtemps qu'il est là
        //Debug.Log("OK");
        toDestroy.CreatureDoing.CreatureEatCreature -= CreatureEatCreature;
        toDestroy.CreatureDoing.CreatureEatFood -= CreatureEatFood;

        /*Debug.Log("--------------CreatureList-------------");
        foreach (int specie in CreatureCountTable)
        {
            Debug.Log(specie);
        }
        Debug.Log("-----------");*/
        //
        Destroy(toDestroy.gameObject);

        Nest nest = nests != null ? nests.FirstOrDefault(n => n.SpecieID == specieID) : null;
        if(AliveCreature.Sum(creature => creature.SpecieID == specieID ? 1 : 0) == 0 && nest){
            List<Creature> creatures = CreateCreaturesFromConfig (specieID, 8);
            foreach(Creature creature in creatures){
                
                creature.transform.parent = IslandGenerator.Instance.transform;
                creature.transform.position = nest.transform.position + Vector3.up * 0.5f;
                creature.gameObject.SetActive(true);
            }
        }
    }

    public void CreatureEatCreature(Creature creature1, Creature creature2)
    {
        MealCreature?.Invoke();
    }

    public void CreatureEatFood(Creature creature, Food food)
    {
        MealCreature?.Invoke();
    }

    public static Action creaturePopulationChangedEvent;

}
