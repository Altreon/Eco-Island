using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : PerceptionView
Permet à l'agent de produire du son qui sera perceptible par d'autre agents
*/

public class SoundEmission 
{
    private Agent owner;
    private float MaxSoundEmissionRadius;
    private AnimationCurve MaxSoundByVelocity;
    private float MaxVelocity = 3.5f;
    public float CurrentSoundEmissionRadius;
    private float EmitRepeatTime;
    private float nextTime = 0.0f;
    private LayerMask CreatureLayerMask = LayerMask.GetMask("Creature");
    private Collider[] CreaturesInSoundEmissionRadius;

    public SoundEmission(Agent owner)
    {
        this.owner = owner;
        this.MaxSoundEmissionRadius = owner.PerceptionConfig.MaxSoundEmissionRadius;
        this.MaxSoundByVelocity = owner.PerceptionConfig.MaxSoundByVelocity;
        this.EmitRepeatTime = owner.PerceptionConfig.EmitRepeatTime;

        CurrentSoundEmissionRadius = MaxSoundEmissionRadius;
    }

    public void EmitSound()
    {
        CreaturesInSoundEmissionRadius = Physics.OverlapSphere(owner.transform.position,CurrentSoundEmissionRadius,CreatureLayerMask);
        
        foreach(Collider CreatureCollider in CreaturesInSoundEmissionRadius)
        {
            Agent agent = CreatureCollider.GetComponent<Agent>();
            if(agent != owner && agent.IsThinking)
            {
                agent.Perception.PerceptionSound.ReceiveData(owner.transform);
            }
        }
    }

    //Pour faire connaitre notre présence au allié même si on bouge pas
    public void EmitSoundForFriend()
    {
        CreaturesInSoundEmissionRadius = Physics.OverlapSphere(owner.transform.position,MaxSoundEmissionRadius,CreatureLayerMask);
        
        foreach(Collider CreatureCollider in CreaturesInSoundEmissionRadius)
        {
            Agent agent = CreatureCollider.GetComponent<Agent>();
            if(agent != owner && agent.IsThinking && agent.Creature.SpecieID == owner.Creature.SpecieID)
            {
                agent.Perception.PerceptionSound.ReceiveData(owner.transform);
            }
        }
    }

    public void Update()
    {
        // Evolution Linéaire du rayon en fonction de la vélocité
        CurrentSoundEmissionRadius = MaxSoundByVelocity.Evaluate(owner.Steering.Velocity.magnitude/MaxVelocity) * MaxSoundEmissionRadius;

        //INFO je me suis permis de commenter le debug
        //Debug.Log(CurrentSoundEmissionRadius);

        if(Time.time > nextTime)
        {
            nextTime = Time.time + EmitRepeatTime;

            // Si l'agent est en mouvement, alors il emet un son
            if(owner.Steering.Velocity.magnitude != 0)
            {
                EmitSound();
            }

            EmitSoundForFriend();
        }
    }
}
