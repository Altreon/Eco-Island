using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostConfig", menuName = "Particularities/GhostConfig", order = 0)]
public class GhostConfig : ParticularityConfig
{
    [Header("Estetics")]
    public float transparency = 0.1f;
    public float appearDuration = 3;
    public float transitionSpeed = 1;
}
