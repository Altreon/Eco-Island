using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerceptionConfig", menuName = "Perception/PerceptionConfig", order = 1)]
public class PerceptionConfig : ScriptableObject
{
    [Header("View")]
    public float viewRadius = 10;
    public float viewAngle = 90;
    public LayerMask ObjectViewLayerMask;
    public LayerMask ObstacleViewLayerMask;

    [Header("Sound")]
    public float MaxSoundEmissionRadius = 5;
    public AnimationCurve MaxSoundByVelocity;
    public float EmitRepeatTime = 3f;
}
