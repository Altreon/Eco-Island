using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SparkConfig", menuName = "Particularities/SparkConfig", order = 0)]
public class SparkConfig : ParticularityConfig
{
    [Header("Estetics")]
    public ParticleType charge;
    public ParticleType spark;
    public float minSize = 1f;
    public float maxSize = 2f;
    public int minEmission = 30;
    public int maxEmission = 100;

    [Header("Damage")]
    public float maxDamageToCreatures = 10;
    public float maxDamageToPlayer = 20;


    [Header("Others")]
    public float distanceWithCreature = 10;
    public float rechargeTime = 10f;
}
