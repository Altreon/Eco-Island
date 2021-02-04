using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : SpeciesMetrics
Affichage de données du monde de jeu sous forme de graphique par dessus le rendu de l'application
*/

public class SpeciesMetrics : MonoBehaviour
{
    public static SpeciesMetrics Instance {get; private set;}
    // Works with regular fields
    //[DebugGUIGraph(min: -1, max: 1, r: 0, g: 1, b: 0, autoScale: true)]
    //float SinField;

    [Min(0)]
    public float updateFreq = 1;
    public CREATURE_TRAITS observedTrait = CREATURE_TRAITS.CARNIVOROUS;

    private CreatureFactory factory;
    private int nbSpecies;

    public bool IsReady {get; private set;} = false;

    public int[] NbCreatures {get; private set;}

    private CREATURE_TRAITS currentObservedTrait;

    private void Awake () {
        if(Instance){
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Destroy () {
        if(Instance == this) Instance = null;
    }

    private void Start() {
        factory = CreatureFactory.Instance;

        if(!factory){
            enabled = false;
            return;
        }

        nbSpecies = factory.configSpawn.Species.Length;
        NbCreatures = new int[nbSpecies];

        /*for(int i = 0; i < nbSpecies; ++i){
            DebugGUI.SetGraphProperties("population " + i, "Species ID : " + i, 0, 10, 0, new Color(1, 0, 0), true);
            DebugGUI.SetGraphProperties("population " + i, "Species ID : " + i, 0, 10, 0, new Color(1, 0, 0), true);
        }*/

        currentObservedTrait = observedTrait;

        StartCoroutine(WaitAndUpdateStats());

        IsReady = true;
    }

    private IEnumerator WaitAndUpdateStats () {
        while(enabled){
            yield return new WaitForSeconds(updateFreq);
            UpdateStats();
        }
    }

    private void UpdateStats () {
        Color[] colors = new Color[nbSpecies];
        int[] nbs = new int[nbSpecies];
        float[] traits = new float[nbSpecies];
        int[][] nbsFoods = new int[nbSpecies][];

        for(int i = 0; i < nbSpecies; ++i){
            nbsFoods[i] = new int[nbSpecies + 2];
        }

        foreach(Creature creature in factory.AliveCreature) {
            int id = creature.SpecieID;
            colors[id] += creature.ColorSwap.GetColor();
            ++nbs[id];
            traits[id] += creature.Traits.Get(observedTrait);

            DataSpecies data = creature.agentCreature.Memory.Species.GetByKey(creature.SpecieID);
            foreach(CarnivorousFood food in data.CarnivorousFoods){
                ++nbsFoods[id][food.preyID];
            }
            foreach(HerbivorFood food in data.HerbivorFoods){
                ++nbsFoods[id][nbSpecies + (food.foodType == FoodType.Fruit ? 0 : 1)];
            }
        }

        for(int i = 0; i < nbSpecies; ++i){
            int nb = nbs[i];
            NbCreatures[i] = nb;

            Color mediumColor = colors[i] / nb;
            float mediumTrait = traits[i] / nb;

            int maxFoodID = 0;
            for(int k = 1; k < nbSpecies + 2; k++) {
                if(nbsFoods[i][k] > nbsFoods[i][maxFoodID]) maxFoodID = k;
            }

            string prefFood;
            if(maxFoodID >= nbSpecies){
                prefFood = maxFoodID == 0 ? "Fruit" : "Vegetable";
            }else{
                prefFood = "Meal of Species " +  maxFoodID;
            }

            DebugGUI.SetGraphProperties("population " + i, "Population of Species " + i, 0, 25, 0, mediumColor, false);
            if(currentObservedTrait != observedTrait) DebugGUI.RemoveGraph("trait " + i);
            DebugGUI.SetGraphProperties("trait " + i, observedTrait + " of Species " + i, 0, 1, 1, mediumColor, false);
            DebugGUI.SetGraphProperties("prefFood " + i, "Favorite of Species " + i + " : " + prefFood, 0, 0, 2, mediumColor, false);

            DebugGUI.Graph("population " + i, nb);
            DebugGUI.Graph("trait " + i, mediumTrait);
        }

        currentObservedTrait = observedTrait;
    }
}
