using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : PerceptionSound
Permet au module de perception de récoltées des informations entendus par l'agent grâce à un cercle autour de l'agent
*/

public class PerceptionSound 
{
    private Agent owner;
    
    public PerceptionSound(Agent owner)
    {
        this.owner = owner;
    }

    // Un son a été entendu provenant de créature
    public void ReceiveData(Transform transmitter)
    {
        //TODO activer pour debuger la perception sonore
        /*
        if(owner.Debug)
        {
            Debug.LogError(owner.transform.name+" a entendu un son provenent de la créature "+transmitter.name);
        }
        */

        LayerMask transmitterLayer = transmitter.gameObject.layer;

        if(transmitterLayer == LayerMask.NameToLayer("Creature"))
        {
            Creature creature = transmitter.gameObject.GetComponent<Creature>();
            owner.Memory.Creatures.Write(new DataCreature(creature,creature.transform.position));
        }

        else if(transmitterLayer == LayerMask.NameToLayer("Player"))
        {        
            owner.Memory.Player.lastSeeTime = Time.time;
        }
    }
}
