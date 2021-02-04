using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Particularity
{
    private ExplosionConfig config;

    private ParticleSystem fuse;

    private float useTime = -1;

    public Explosion (Creature owner, ExplosionConfig config) : base(owner) {
        this.config = config;
    }

    public override bool PrepareCondition () {
        System.Type goalType = owner.agentCreature.Thinking.ActiveGoal?.GetType();

        return (goalType == typeof(GoalDefensePlayer) || goalType == typeof(GoalDefense)) && IsRecharged();
    }

    public override void Prepare () {
       if(!fuse) {
            fuse = ParticuleManager.Instance.CreateParticle(config.fuse, owner.transform.position + owner.transform.up * 0.7f, owner.transform.rotation).GetComponent<ParticleSystem>();
        }

        fuse.transform.position = owner.transform.position + owner.transform.up * 0.7f;
    }

    public override bool ActivationCondition () {
        GoalComposite goal = owner.agentCreature.Thinking.ActiveGoal as GoalComposite;
        System.Type subGoalType = goal?.GetActiveGoal()?.GetType();

        if(goal?.GetType() == typeof(GoalDefensePlayer)){
            return IsRecharged()
               && (subGoalType == typeof(GoalPursuitPlayer) || subGoalType == typeof(GoalAttackPlayer))
               && Vector3.Distance(owner.transform.position, Player.Instance.transform.position) <= config.minDistWithOpponentToExplode;
        }else if(goal?.GetType() == typeof(GoalDefense)){
            return IsRecharged()
               && (subGoalType == typeof(GoalPursuit) || subGoalType == typeof(GoalAttack))
               && owner.agentCreature.Steering.Target
               && Vector3.Distance(owner.transform.position, owner.agentCreature.Steering.Target.transform.position) <= config.minDistWithOpponentToExplode;
        }else{
            return false;
        }
    }
    public override void Activation () {
        if(fuse) {
            ParticuleManager.Instance.DestroyParticle(ParticleType.ParticularityFuse, fuse.gameObject);
            fuse = null;
        }

        Vector3 ownerPos = owner.transform.position;

        ParticuleManager.Instance.CreateParticle(config.explosion, ownerPos, Quaternion.identity);

        Collider[] hitColliders = Physics.OverlapSphere(ownerPos, config.radius, LayerMask.GetMask("Creature"));
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject != owner.gameObject) {
                Vector3 impactPoint = hitCollider.ClosestPoint(ownerPos);
                float dist = Vector3.Distance(ownerPos, impactPoint);
                float distanceFactor = ( Mathf.Pow(dist+1, 2) - Mathf.Pow(config.radius+1, 2)) / (1 - Mathf.Pow(config.radius+1, 2));

                Creature creature = hitCollider.GetComponent<Creature>();
                if(creature.SpecieID != owner.SpecieID) creature.Gauges.Life.Value -= Mathf.RoundToInt(config.damageToCreatures * distanceFactor);
                creature.agentCreature.Throw(((impactPoint - Vector3.down * 3f) - ownerPos).normalized, config.force * distanceFactor);
                //hitCollider.attachedRigidbody.AddForce(config.force * distanceFactor * (impactPoint - ownerPos).normalized, ForceMode.Impulse);
            }
        }

        float distance = Vector3.Distance(ownerPos, Player.Instance.transform.position);

        if(distance <= config.radius) {
            float distanceFactor = ( Mathf.Pow(distance+1, 2) - Mathf.Pow(config.radius+1, 2)) / (1 - Mathf.Pow(config.radius+1, 2));
            Player.Instance.PlayerHealth.Health -= config.damagesToPlayer * distanceFactor;
            //Player.Instance.PlayerGhost.Throw(((Player.Instance.transform.position - Vector3.down) - ownerPos).normalized, config.force * distanceFactor);
        }

        useTime = Time.time;
    }

    public override void Inactif () {
        if(fuse) {
            ParticuleManager.Instance.DestroyParticle(ParticleType.ParticularityFuse, fuse.gameObject);
            fuse = null;
        }
    }

    private bool IsRecharged () {
        return useTime == -1 || Time.time - useTime >= config.rechargeTime;
    }

    public override void Destroy () {
        Inactif();
    }
}
