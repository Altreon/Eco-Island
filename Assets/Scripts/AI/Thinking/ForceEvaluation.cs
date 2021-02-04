using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/**
Classe : ForceEvaluation
Classe à améliorer, qui permet de pondéré des fonctions de désirabilités selon un rapport de force entre
alliés et ennemis présents (et perçus) à proximité
*/

public class ForceEvaluation
{
    //TODO ultra basique pour le moment, à complexifier quand les traites et jauges seront prêtes

    //TODO config
    public static float weight = 0.2f;
    public static float weightPlayer = 0.5f;
    
    public static float Evaluate (Agent evaluator, Creature target) {
        return (1 - weight) + Ratio(evaluator, target) * weight;
        //return 100;
    }

    public static float EvaluateAgainstPlayer (Agent evaluator) {
        return (1 - weightPlayer) + Ratio(evaluator) * weightPlayer;
        //return 100;
    }

    public static float Ratio (Agent evaluator, Creature target) {
        Creature evaluatorCreature = evaluator.Creature;

        //TODO à utiliser plus tard, pour la complexificaiton
        DataSpecies evaluatorSpecies = evaluator.Memory.Species.GetByKey(evaluator.Creature.SpecieID);
        DataSpecies targetSpecies = target.agentCreature.Memory.Species.GetByKey(evaluator.Creature.SpecieID);
        if(evaluatorSpecies == null || targetSpecies == null) return 1;

        int nbAllies = 1;
        int nbEnnemies = 1;

        IEnumerable<DataCreature> creatures = evaluator.Memory.Creatures.Read();
        foreach(DataCreature data in creatures){
            Creature creature = data.creature;
            if(!creature || !creature.gameObject.activeSelf || data.RegistrationDate < Time.time - 5f) continue;
            if(creature.agentCreature == evaluator || creature == target) continue;

            int specieID = creature.SpecieID;
            if(specieID == evaluatorCreature.SpecieID){
                ++nbAllies;
            }
            else if(specieID == target.SpecieID){
                ++nbEnnemies;
            }
        }

        return nbAllies / nbEnnemies;
    }

    public static float Ratio (Agent evaluator) {
        Creature evaluatorCreature = evaluator.Creature;

        //TODO à utiliser plus tard, pour la complexificaiton
        DataSpecies evaluatorSpecies = evaluator.Memory.Species.GetByKey(evaluator.Creature.SpecieID);
        if(evaluatorSpecies == null) return 1;

        int nbAllies = 1;
        int nbEnnemies = 1;

        IEnumerable<DataCreature> creatures = evaluator.Memory.Creatures.Read();
        foreach(DataCreature data in creatures){
            Creature creature = data.creature;
            if(!creature || !creature.gameObject.activeSelf || data.RegistrationDate < Time.time - 5f) continue;
            if(creature.agentCreature == evaluator) continue;

            int specieID = creature.SpecieID;
            if(specieID == evaluatorCreature.SpecieID){
                ++nbAllies;
            }else if(creature.agentCreature.Thinking.ActiveGoal is GoalProtectPlayer){
                ++nbEnnemies;
            }
        }

        return nbAllies / nbEnnemies;
    }
}
