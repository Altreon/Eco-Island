using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RevisionConfig", menuName = "AI/RevisionConfig", order = 0)]
public class RevisionConfig : ScriptableObject
{
    [Range(0, 1)]
    public float InfluenceProportion = 0.1f;

    public CreatureSeeCreatureRevision CreatureSeeCreatureRevision;
    public CreatureDeadRevision CreatureDeadRevision;
    public CreatureAttackRevision CreatureAttackRevision;
    public CreatureEatRevision CreatureEatRevision;
    public CreatureEatFoodRevision CreatureEatFoodRevision;
    public CreatureReproduceRevision CreatureReproduceRevision;
    public CreatureCommunicateRevision CreatureCommunicateRevision;
    public CreaturePursuitRevision CreaturePursuitRevision;
    public CreaturePursuitPlayerRevision CreaturePursuitPlayerRevision;
    public CreatureEvadePlayerRevision CreatureEvadePlayerRevision;
    public CreatureEatPlayerFoodRevision CreatureEatPlayerFoodRevision;
    public CreatureFlashedByPlayerRevision CreatureFlashedByPlayerRevision;
}
