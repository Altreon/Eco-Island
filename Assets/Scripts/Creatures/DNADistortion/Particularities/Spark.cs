using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spark : Particularity
{
    private SparkConfig config;

    private ParticleSystem charge;
    private AudioSource chargeSound;
    private Light chargeLight;

    float minDistWithSparker = -1;

    private float useTime = -1;

    public Spark (Creature owner, SparkConfig config) : base(owner) {
        this.config = config;
    }

    public override bool PrepareCondition () {
        minDistWithSparker = -1;

        if(!IsRecharged()) return false;

        if(owner.currentEmotion == EmotionState.Sleep) return false;
        if(!owner.agentCreature.IsThinking) return false;

        foreach(Creature creature in CreatureFactory.Instance.AliveCreature) {
            if(creature == owner) continue;

            float distance = Vector3.Distance(owner.transform.position, creature.transform.position);
            if(minDistWithSparker != -1 && distance >= minDistWithSparker) continue;
            if(!creature.DNADistortion.HaveParticularity(typeof(Spark))) continue;

            minDistWithSparker = distance;
        }

        return minDistWithSparker != -1 && minDistWithSparker < config.distanceWithCreature;
    }

    public override void Prepare () {
        if(!charge) {
            charge = ParticuleManager.Instance.CreateParticle(config.charge, owner.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            charge.transform.parent = owner.transform;

            chargeSound = charge.GetComponent<AudioSource>();
            chargeSound.time = Random.Range(0, chargeSound.clip.length);

            chargeLight = charge.GetComponent<Light>();
        }

        float ratio = 1 - minDistWithSparker / config.distanceWithCreature;

        charge.transform.localScale = Vector3.one * Mathf.Lerp(config.minSize, config.maxSize, ratio);

        ParticleSystem.EmissionModule emission = charge.emission;
        emission.rateOverTime = Mathf.Lerp(config.minEmission, config.maxEmission, ratio);

        chargeSound.volume = ratio;

        chargeLight.intensity = Mathf.Lerp(1, 5, ratio);
    }

    public override bool ActivationCondition () {
        if(!IsRecharged()) return false;
        if(minDistWithSparker == -1 || minDistWithSparker > config.distanceWithCreature) return false;

        Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, 2, LayerMask.GetMask("Creature", "Player"));
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject != owner.gameObject) {
                if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Creature")){
                    Creature creature = hitCollider.GetComponent<Creature>();
                    if(!creature || creature.SpecieID == owner.SpecieID) continue;

                    return true;
                }else if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Player")){
                    return true;
                }
            }
        }

        return false;
    }

    public override void Activation () {
        float ratio = 1 - minDistWithSparker / config.distanceWithCreature;

        ParticuleManager.Instance.CreateParticle(config.spark, owner.transform.position, Quaternion.identity);

        Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, 2, LayerMask.GetMask("Creature", "Player"));
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject != owner.gameObject) {
                if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Creature")){
                    Creature creature = hitCollider.GetComponent<Creature>();
                    if(!creature || creature.SpecieID == owner.SpecieID) continue;

                    creature.Gauges.Life.Value -= Mathf.RoundToInt(Mathf.Lerp(0, config.maxDamageToCreatures, ratio));
                }else if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Player")){
                     Player.Instance.PlayerHealth.Health -= Mathf.RoundToInt(Mathf.Lerp(0, config.maxDamageToPlayer, ratio));
                }
            }
        }

        useTime = Time.time;
    }

    public override void Inactif () {
        if(charge) {
            ParticuleManager.Instance.DestroyParticle(ParticleType.ParticularityCharge, charge.gameObject);
            charge = null;
            chargeSound = null;
            chargeLight = null;
        }
    }

    private bool IsRecharged () {
        return useTime == -1 || Time.time - useTime >= config.rechargeTime;
    }

    public override void Destroy () {
        Inactif();
    }
}
