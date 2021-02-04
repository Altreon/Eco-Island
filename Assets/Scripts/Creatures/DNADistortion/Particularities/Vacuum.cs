using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : Particularity
{
    private VacuumConfig config;

    private GameObject vacuum;

    public float useTime {get; private set;} = -1;
    public bool Actif {get; set;} = false;

    public Vacuum (Creature owner, VacuumConfig config) : base(owner) {
        this.config = config;
    }

    public override bool PrepareCondition () {
        return false;
    }

    public override void Prepare () {
       
    }

    public override bool ActivationCondition () {
        return Actif;
    }

    public override void Activation () {
        float taille = Mathf.Lerp(owner.transform.localScale.x, owner.SizeForAge * config.inflateSizeFactor, config.inflateSpeed * Time.deltaTime);
        owner.transform.localScale = Vector3.one * taille;

        Vector3 header = Vector3.ProjectOnPlane(owner.transform.forward, Vector3.up).normalized;

        Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, config.distance, LayerMask.GetMask("Creature", "Player", "Food"));
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject != owner.gameObject) {
                if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Creature")){
                    Creature creature = hitCollider.GetComponent<Creature>();
                    if(!creature || creature.SpecieID == owner.SpecieID) continue;

                    Vector3 toCreature = Vector3.ProjectOnPlane(creature.transform.position - owner.transform.position, Vector3.up).normalized;
                    if(Vector3.Angle(header, toCreature) > config.angle) continue;

                    creature.transform.position += -toCreature * config.attractionSpeed * Time.deltaTime;
                }else if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Player")){
                     Vector3 toCreature = Vector3.ProjectOnPlane(Player.Instance.PlayerController.transform.position - owner.transform.position, Vector3.up).normalized;
                    if(Vector3.Angle(header, toCreature) > config.angle) continue;

                    Player.Instance.PlayerController.ForceMove(-toCreature * config.attractionSpeed);
                }else{
                    Vector3 toCreature = Vector3.ProjectOnPlane(hitCollider.transform.position - owner.transform.position, Vector3.up).normalized;
                    if(Vector3.Angle(header, toCreature) > config.angle) continue;
                    
                    hitCollider.attachedRigidbody.AddForce(-toCreature * config.attractionSpeed * 5, ForceMode.Acceleration);
                }
            }
        }
    }

    public void Active () {
        if(!vacuum) {
            vacuum = ParticuleManager.Instance.CreateParticle(config.vacuum, owner.transform.position + owner.transform.forward * 0.45f, owner.transform.rotation);
            vacuum.transform.parent = owner.transform;
        }

        owner.currentEmotion = EmotionState.Swallow;
        owner.FaceSwap.Swap(EmotionState.Swallow);

        owner.UpdateSize = false;
        Actif = true;
    }

    public void Desactive () {
        if(vacuum) {
            ParticuleManager.Instance.DestroyParticle(ParticleType.ParticularityVacuum, vacuum.gameObject);
            vacuum = null;
        }

        Actif = false;
        useTime = Time.time;
    }

    public override void Inactif () {
        if(owner.transform.localScale.x > owner.SizeForAge + 0.1f) {
            float taille = Mathf.Lerp(owner.transform.localScale.x, owner.SizeForAge, config.inflateSpeed * Time.deltaTime);
            owner.transform.localScale = Vector3.one * taille;

        }else if(!owner.UpdateSize){
            owner.UpdateSize = true;
        }
    }

    public bool IsRecharged () {
        return useTime == -1 || Time.time - useTime >= config.rechargeTime;
    }

    public override void Destroy () {
        Desactive();
    }
}
