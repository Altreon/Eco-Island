using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VacuumConfig", menuName = "Particularities/VacuumConfig", order = 0)]
public class VacuumConfig : ParticularityConfig
{
    [Header("Estetics")]
    public ParticleType vacuum = ParticleType.ParticularityVacuum;
    public float inflateSpeed = 1;
    public float inflateSizeFactor = 3;
    
    [Header("Physics")]
    public float distance = 12;
    public float attractionSpeed = 4f;
    public float angle = 45f;

    [Header("Others")]
    public float rechargeTime = 20;
}
