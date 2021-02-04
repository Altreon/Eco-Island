using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking
{
    private static Attacking _instance;
    public static Attacking Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Attacking();
            }
            return _instance;
        }
    }

    public void Attack(Creature agressor, Creature victim)
    {
        if (agressor == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement Attack alors que la créature appelante est vide");
            return;
        }
        if (victim == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement Attack sur une cible Vide");
            return;
        }
        //Object.Instantiate(ParticuleManager.Instance.BulletExplosionFire, victim.transform.position, Quaternion.identity);
        ParticuleManager.Instance.CreateParticle(ParticleType.BulletExplosionFire, victim.transform.position, Quaternion.identity);

        //ajout d'un stimuli
        victim.agentCreature.Memory.Creatures.Write(new DataCreature(agressor, agressor.transform.position));
        if(victim.agentCreature != null && victim.agentCreature.IsThinking){
            Revision.Instance.ReviseCreatureAttack(victim.agentCreature, agressor, victim);
        }

        //ajout de son
        agressor.AudioBox.PlayOneShot(SoundOneShot.CreatureAttack);

        if(victim.agentCreature.Steering.Target == agressor.agentCreature){
            victim.AudioBox.PlayOneShot(SoundOneShot.CreatureDamagedWithResistance);
        }else{
            victim.AudioBox.PlayOneShot(SoundOneShot.CreatureDamagedWithoutResistance);
        }

        //victim.LifeNumber -= agressor.attackPower;
        victim.Gauges.Life.Value -= Mathf.RoundToInt(agressor.Traits.Strength.Value * 50);

        if(agressor.DNADistortion.HaveParticularity(typeof(Venom))){
            (agressor.DNADistortion.GetParticularity(typeof(Venom)) as Venom).ActiveOnCreature(victim);
        }
    }

    public void AttackPlayer(Creature agressor)
    {
        Player player = Player.Instance;

        if (agressor == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement Attack alors que la créature appelante est vide");
            return;
        }
        if (player == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement Attack sur un joueur vide");
            return;
        }
        //Object.Instantiate(ParticuleManager.Instance.BulletExplosionFire, victim.transform.position, Quaternion.identity);
        ParticuleManager.Instance.CreateParticle(ParticleType.BulletExplosionFire, player.transform.position, Quaternion.identity);

        //ajout de son
        agressor.AudioBox.PlayOneShot(SoundOneShot.CreatureAttack);

        if( Player.Instance.PlayerPickAndDrop.IsHandleFood)
        {
            Player.Instance.PlayerPickAndDrop.DropOffFood();
        }

        //TODO reduire la vie du player
        //player.Life -= Mathf.RoundToInt(agressor.Traits.Strength.Value * 50);
        Player.Instance.PlayerHealth.Health -= Mathf.RoundToInt(agressor.Traits.Strength.Value * 50);



        if(agressor.DNADistortion.HaveParticularity(typeof(Venom))){
            (agressor.DNADistortion.GetParticularity(typeof(Venom)) as Venom).ActiveOnPlayer();
        }
    }

    public static bool IsVictimWillDie (Creature victim, Creature agressor){
        return victim.Gauges.Life.Value <= Mathf.RoundToInt(agressor.Traits.Strength.Value * 50);
    }
}
