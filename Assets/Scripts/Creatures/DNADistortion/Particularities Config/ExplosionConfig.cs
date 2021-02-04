using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionConfig", menuName = "Particularities/ExplosionConfig", order = 0)]
public class ExplosionConfig : ParticularityConfig
{
    [Header("Estetics")]
    public ParticleType fuse;
    public ParticleType explosion;

    [Header("Damages")]
    public float radius;
    public float damageToCreatures;
    public float damagesToPlayer;

    [Header("Physics")]
    public float force;

    [Header("Others")]
    public float rechargeTime;
    public float minDistWithOpponentToExplode;
}
