using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RouliConfig", menuName = "Particularities/RouliConfig", order = 0)]
public class RouliConfig : ParticularityConfig
{
    [Header("Estetics")]
    public float rotateSpeed = 1f;
}
