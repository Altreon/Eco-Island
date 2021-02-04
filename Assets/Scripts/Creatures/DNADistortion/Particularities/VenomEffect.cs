using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomEffect : MonoBehaviour
{

    private GameObject effect;
    private GameObject effect2;
    private float duration;
    private float damagePerSecond;

    private bool isTargetCreature = false;
    private Creature target = null;

    private float lastDamageTime;
    private float creationTime;

    public void SetupForPlayer (GameObject effect, GameObject effect2, float duration, float damagePerSecond){
        this.effect = effect;
        this.effect2 = effect2;
        this.duration = duration;
        this.damagePerSecond = damagePerSecond;

        creationTime = Time.time;
    }

    public void SetupForCreature (GameObject effect, float duration, float damagePerSecond, Creature target){
        this.effect = effect;
        this.duration = duration;
        this.damagePerSecond = damagePerSecond;
        this.target = target;

        creationTime = Time.time;
        isTargetCreature = true;
    }

    public void ResetDuration () {
        creationTime = Time.time;
    }

    public void Update () {
        if(Time.time - creationTime > duration) {
            Destroy(this);
            return;
        }

        bool damage = false;
        if(Time.time - lastDamageTime > 1 / damagePerSecond){
            damage = true;
            lastDamageTime = Time.time;
        }

        if(isTargetCreature){
            effect.transform.position = target.transform.position + Vector3.up * 0.3f;
            if(damage) --target.Gauges.Life.Value;
        }else{
            if(damage) --Player.Instance.PlayerHealth.Health;
        }
    }

    private void OnDestroy () {
        if(isTargetCreature){
            if(ParticuleManager.Instance) ParticuleManager.Instance.DestroyParticle(ParticleType.ParticularityVenomCreature, effect);
        }else{
            if(ParticuleManager.Instance) ParticuleManager.Instance.DestroyParticle(ParticleType.ParticularityVenomPlayer, effect);
            if(ParticuleManager.Instance) ParticuleManager.Instance.DestroyParticle(ParticleType.ParticularityVenomCreature, effect2);
        }
    }
}
