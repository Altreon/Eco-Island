using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VenomConfig", menuName = "Particularities/VenomConfig", order = 0)]
public class VenomConfig : ParticularityConfig
{
    [Header("Estetics")]
    public ParticleType poisonForCreature;
    public ParticleType poisonForPlayer;

    [Header("Damages")]
    public float duration = 5;
    public float damagePerSecondsToCreatures = 1;
    public float damagesPerSecondsToPlayer = 5;
}
