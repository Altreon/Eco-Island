using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DesirabilitiesConfig", menuName = "AI/DesirabilitiesConfig", order = 0)]
public class DesirabilitiesConfig : ScriptableObject
{
    [Header("Wander")]
    public float WanderDesirabilityValue = 0.05f;

    [Header("Evade")]
    public AnimationCurve EvadeDesirabilityByDistance;
    public AnimationCurve EvadeDesirabilityByAggressivity;
    [Min(0)]
    public float EvadeConsiderationMaxDistance = 30f;
    [Min(0)]
    public float EvadeDesirabilityTwicker = 1f;
    [Min(1)]
    public float EvadeConfirmationBias = 1f;

    [Header("EvadePlayer")]
    public AnimationCurve EvadePlayerDesirabilityByDistance;
    public AnimationCurve EvadePlayerDesirabilityByAggressivity;
    public AnimationCurve EvadePlayerDesirabilityByVigilance;
    [Min(0)]
    public float EvadePlayerConsiderationMinVigilance = 0.3f;
    [Min(0)]
    public float EvadePlayerConsiderationMaxDistance = 30f;
    [Min(0)]
    public float EvadePlayerDesirabilityTwicker = 1f;
    [Min(1)]
    public float EvadePlayerConfirmationBias = 1f;
    [Min(0)]
    public float EvadePlayerDesirabilityTwickerWithFoodHolder = 1f;

    [Header("Hungry")]
    public AnimationCurve HungryDesirabilityByStomack;
    public AnimationCurve HungryDesirabilityByGluttony;
    [Min(0)]
    public float HungryDesirabilityTwicker = 1f;
    [Min(1)]
    public float HungryConfirmationBias = 1f;

    [Header("Reproduction")]
    public AnimationCurve ReproductionDesirabilityByTimer;
    public AnimationCurve ReproductionDesirabilityByLust;
    [Min(0)]
    public float ReproductionDesirabilityTwicker = 1f;
    [Min(1)]
    public float ReproductionConfirmationBias = 1f;
    [Min(0)]
    public float ReproductionRequestBias = 1f;

    [Header("Defense")]
    //public AnimationCurve DefenseDesirabilityByDistance;
    public AnimationCurve DefenseDesirabilityByAggressivity;
    [Min(0)]
    public float DefenseDesirabilityTwicker = 1f;
    [Min(1)]
    public float DefenseConfirmationBias = 1f;
    [Min(0)]
    public float DefenseMaxTimeSpend = 10f;
    [Min(0)]
    public float DefenseMinTimeRepeat = 5f;

    [Header("DefensePlayer")]
    public AnimationCurve DefensePlayerDesirabilityByAggressivity;
    public AnimationCurve DefensePlayerDesirabilityByVigilance;
    [Min(0)]
    public float DefensePlayerConsiderationMinVigilance = 0.3f;
    [Min(0)]
    public float DefensePlayerDesirabilityTwicker = 1f;
    [Min(1)]
    public float DefensePlayerConfirmationBias = 1f;
    [Min(0)]
    public float DefensePlayerPlayerSpeedThreatWeight = 0.5f;
    [Min(0)]
    public float DefensePlayerDesirabilityTwickerWithFoodHolder = 1f;
    [Min(0)]
    public float DefensePlayerMaxTimeSpend = 10f;
    [Min(0)]
    public float DefensePlayerMinTimeRepeat = 5f;

    [Header("Communication")]
    public AnimationCurve CommunicationDesirabilityByTime;
    public AnimationCurve CommunicationDesirabilityBySociability;
    [Min(0)]
    public float CommunicationConsiderationMaxTime = 60f;
    [Min(0)]
    public float CommunicationDesirabilityTwicker = 1f;
    public float CommunicationConfirmationBias = 1f;
    [Min(0)]
    public float CommunicationRequestBias = 1f;
    //Doivent être trier du moins probable au plus probable
    public List<SubjectPropability> CommunicationSubjetPropabilities = null;

    [Header("Reach Nest")]
    public AnimationCurve ReachNestDesirabilityByDistance;
    [Min(0)]
    public float ReachNestConsiderationMaxDistance = 500f;
    [Min(0)]
    public float ReachNestDesirabilityTwicker = 1f;
    [Min(1)]
    public float ReachNestConfirmationBias = 1f;

    [Header("Avoid Prey")]
    public AnimationCurve AvoidPreyDesirabilityByDistance;
    public float AvoidPreyConsiderationMaxDistance = 30f;
    [Min(0)]
    public float AvoidPreyDesirabilityTwicker = 1f;
    [Min(1)]
    public float AvoidPreyConfirmationBias = 1f;

    [Header("Observe Player")]
    public AnimationCurve ObservePlayerDesirabilityByDistance;
    public AnimationCurve ObservePlayerDesirabilityByVigilance;
    public AnimationCurve ObservePlayerDesirabilityByCuriosiry;
    [Min(0)]
    public float ObservePlayerConsiderationMaxVigilance = 0.3f;
    [Min(0)]
    public float ObservePlayerConsiderationMaxDistance = 15f;
    [Min(0)]
    public float ObservePlayerDesirabilityTwicker = 1f;
    [Min(1)]
    public float ObservePlayerConfirmationBias = 1f;
    [Min(0)]
    public float ObservePlayerDesirabilityTwickerWithFoodHolder = 1f;
    [Min(0)]
    public float ObservePlayerDesirabilityTwickerWithGiveFood = 1f;
    [Min(0)]
    public float ObservePlayerMaxTimeSpend = 10f;
    [Min(0)]
    public float ObservePlayerMinTimeRepeat = 40f;

    [Header("Sleep")]
    public AnimationCurve SleepDesirabilityByCycleTime;
    public AnimationCurve SleepToNestDesirabilityBySociability;
    [Min(0)]
    public float SlepConsiderationMinNestDistance = 5f;
    [Min(0)]
    public float SlepConsiderationMaxNestDistance = 30f;
    [Min(0)]
    public float SleepDesirabilityTwicker = 1f;
    [Min(1)]
    public float SleepConfirmationBias = 1f;

    [Header("EatInPlayerHand")]
    public AnimationCurve EatInPlayerHandDesirabilityByAggressivity;
    public AnimationCurve EatInPlayerHandDesirabilityByVigilance;
    [Min(0)]
    public float EatInPlayerHandDesirabilityTwicker = 1f;
    [Min(1)]
    public float EatInPlayerHandConfirmationBias = 1f;

    [Header("ProtectPlayer")]
    public AnimationCurve ProtectPlayerDesirabilityByAggressivity;
    public AnimationCurve ProtectPlayerDesirabilityByVigilance;
    [Min(0)]
    public float ProtectPlayerConsiderationMaxVigilance = 0.3f;
    [Min(0)]
    public float ProtectPlayerDesirabilityTwicker = 1f;
    [Min(1)]
    public float ProtectPlayerConfirmationBias = 1f;
}
