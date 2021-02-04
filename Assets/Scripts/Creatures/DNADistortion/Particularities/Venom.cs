using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venom : Particularity
{
    private VenomConfig config;

    public Venom (Creature owner, VenomConfig config) : base(owner) {
        this.config = config;
    }

    public override bool PrepareCondition () {
        return false;
    }

    public override void Prepare () {
       
    }

    public override bool ActivationCondition () {
        return false;
    }

    public void ActiveOnCreature(Creature creature) {
        VenomEffect existVenomEffect = creature.GetComponent<VenomEffect>();
        if(existVenomEffect){
            existVenomEffect.ResetDuration();
        }else{
            GameObject effect = ParticuleManager.Instance.CreateParticle(config.poisonForCreature, creature.transform.position + Vector3.up * 0.3f, Quaternion.identity);

            creature.gameObject.AddComponent<VenomEffect>().SetupForCreature(effect, config.duration, config.damagePerSecondsToCreatures, creature);
        }
    }

    public void ActiveOnPlayer () {
        VenomEffect existVenomEffect = Player.Instance.GetComponent<VenomEffect>();
        if(existVenomEffect){
            existVenomEffect.ResetDuration();
        }else{
            GameObject effect = ParticuleManager.Instance.CreateParticle(config.poisonForPlayer, Vector3.zero, Quaternion.identity);
            effect.transform.parent = CameraController.Instance.transform;
            GameObject effect2 = ParticuleManager.Instance.CreateParticle(config.poisonForPlayer, Vector3.zero, Quaternion.identity);
            effect2.transform.parent = CameraController.Instance.transform;

            effect.transform.localPosition = new Vector3(-0.6f, -0.41f, 0.36f);
            effect2.transform.localPosition = new Vector3(0.6f, -0.41f, 0.36f);
            
            Player.Instance.gameObject.AddComponent<VenomEffect>().SetupForPlayer(effect, effect2, config.duration, config.damagesPerSecondsToPlayer);
        }
    }

    public override void Activation () {

        
    }

    public override void Inactif () {
        
    }

    public override void Destroy () {
        
    }
}
