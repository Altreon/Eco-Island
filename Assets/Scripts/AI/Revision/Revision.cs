using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Revision
Module de révision, qui permet à l'agent de modifier son états de ces connaissances et ses traites selon les situations perçus
*/

public class Revision
{
    private static Revision instance;
    public static Revision Instance {
        get{
            if(instance == null){
                instance = new Revision();
            }

            return instance;
        }
    }

    public Revision () {}

    public void ReviseCreatureSeeCreature (Agent reviseur, Creature saw) {
        GameManager.Instance.RevisionConfig.CreatureSeeCreatureRevision.Revise(reviseur, saw);
    }

    public void ReviseCreatureDead (Agent reviseur, Creature deadCreature) {
        GameManager.Instance.RevisionConfig.CreatureDeadRevision.Revise(reviseur, deadCreature);
    }

    public void ReviseCreatureAttack (Agent reviseur, Creature from, Creature to) {
        GameManager.Instance.RevisionConfig.CreatureAttackRevision.Revise(reviseur, from, to);
    }

    public void ReviseCreatureEatCreature (Agent reviseur, Creature from, Creature to) {
        GameManager.Instance.RevisionConfig.CreatureEatRevision.Revise(reviseur, from, to);
    }

    public void ReviseCreatureEatFood (Agent reviseur, Creature from, FoodType foodType) {
       GameManager.Instance.RevisionConfig.CreatureEatFoodRevision.Revise(reviseur, from, foodType);
    }

    public void ReviseCreatureEatPlayerFood (Agent reviseur, Creature from, FoodType foodType) {
       GameManager.Instance.RevisionConfig.CreatureEatPlayerFoodRevision.Revise(reviseur, from, foodType);
    }

    public void ReviseCreatureReproduce (Agent reviseur, Creature partner1, Creature partner2) {
        GameManager.Instance.RevisionConfig.CreatureReproduceRevision.Revise(reviseur, partner1, partner2);
    }

    public void ReviseCreatureCommunicate (Agent reviseur, Creature partner1, Creature partner2) {
        GameManager.Instance.RevisionConfig.CreatureCommunicateRevision.Revise(reviseur, partner1, partner2);
    }

    public void ReviseCreaturePursuit (Agent reviseur, Creature from, Creature to) {
        GameManager.Instance.RevisionConfig.CreaturePursuitRevision.Revise(reviseur, from, to);
    }

    public void ReviseCreaturePursuitPlayer (Agent reviseur, Creature from) {
        GameManager.Instance.RevisionConfig.CreaturePursuitPlayerRevision.Revise(reviseur, from);
    }

    public void ReviseCreatureEvadePlayer (Agent reviseur, Creature from) {
        GameManager.Instance.RevisionConfig.CreatureEvadePlayerRevision.Revise(reviseur, from);
    }

    public void ReviseCreatureFlashedByPlayer (Agent reviseur, Creature from) {
        GameManager.Instance.RevisionConfig.CreatureFlashedByPlayerRevision.Revise(reviseur, from);
    }
}
