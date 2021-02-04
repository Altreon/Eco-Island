using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private float MaxSoundEmissionRadius;
    private AnimationCurve MaxSoundByVelocity;
    public float CurrentSoundEmissionRadius;
    private float EmitRepeatTime;
    private float nextTime = 0.0f;
    private LayerMask CreatureLayerMask;
    private Collider[] CreaturesInSoundEmissionRadius;

    private void Start()
    {
        this.MaxSoundEmissionRadius = Player.Instance.PlayerPreset.MaxSoundEmissionRadius;
        this.MaxSoundByVelocity = Player.Instance.PlayerPreset.MaxSoundByVelocity;
        this.EmitRepeatTime = Player.Instance.PlayerPreset.EmitRepeatTime;

        CurrentSoundEmissionRadius = MaxSoundEmissionRadius;

        CreatureLayerMask = LayerMask.GetMask("Creature");
    }
    private void Update()
    {
        CurrentSoundEmissionRadius = MaxSoundByVelocity.Evaluate(Player.Instance.PlayerController.MoveVector.sqrMagnitude) * MaxSoundEmissionRadius;

        if(Time.time > nextTime)
        {
            nextTime = Time.time + EmitRepeatTime;

            // Si l'agent est en mouvement, alors il emet un son
            if(Player.Instance.PlayerController.currentSpeed != 0)
            {           
                EmitSound();
            }
        }
    }

    private void EmitSound()
    {
        CreaturesInSoundEmissionRadius = Physics.OverlapSphere(transform.position,CurrentSoundEmissionRadius,CreatureLayerMask);

        foreach(Collider CreatureCollider in CreaturesInSoundEmissionRadius)
        {
            Agent agent = CreatureCollider.GetComponent<Agent>();
            if(agent.IsThinking)
            {               
                agent.Perception.PerceptionSound.ReceiveData(transform);
            }
        }
    }
}
