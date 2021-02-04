using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Player/PlayerConfig", order = 1)]
public class PlayerConfig : ScriptableObject
{
    [Header("First Person Movement")]
    [Min(0)]
    public float walkSpeed = 5;
    [Min(0)]
    public float runSpeed = 7;

    public float slowFactor = 10;
   
    [Header("Physics")]
    [Min(0)]
    public float gravity = 9.81f;
    [Min(0)]
    public float jumpForce = 20;

    [Header("Sound")]
    public float MaxSoundEmissionRadius = 5;
    public AnimationCurve MaxSoundByVelocity;
    public float EmitRepeatTime = 3f;

    [Header("Health")]
    public float MaxHealth = 300;
    public float TimeToRecover = 15;
    public float RecoverSpeed = 1;

    [Header("Death")]
    public float deadAnimationTime = 3.0f;
}
