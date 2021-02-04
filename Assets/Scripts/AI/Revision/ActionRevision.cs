using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionRevision
{
    [SerializeField]
    private RevisionTraits revisionTraits = null;

    protected void Revise (Agent reviseur) {
        CreatureTraits traits = reviseur.Creature.Traits;
        float influenceProportion = GameManager.Instance.RevisionConfig.InfluenceProportion;

        if(revisionTraits.Aggressivity.Value != 0)  traits.Aggressivity.Value = Mathf.Lerp(traits.Aggressivity, revisionTraits.Aggressivity.Value < 0 ? 0 : 1, influenceProportion * Mathf.Abs(revisionTraits.Aggressivity.Value));
        if(revisionTraits.Carnivorous.Value != 0)  traits.Carnivorous.Value = Mathf.Lerp(traits.Carnivorous, revisionTraits.Carnivorous.Value < 0 ? 0 : 1, influenceProportion * Mathf.Abs(revisionTraits.Carnivorous.Value));
        if(revisionTraits.Constitution.Value != 0)  traits.Constitution.Value = Mathf.Lerp(traits.Constitution, revisionTraits.Constitution.Value < 0 ? 0 : 1, influenceProportion * Mathf.Abs(revisionTraits.Constitution.Value));
        if(revisionTraits.Vigilance.Value != 0)  traits.Vigilance.Value = Mathf.Lerp(traits.Vigilance, revisionTraits.Vigilance.Value < 0 ? 0 : 1, influenceProportion * Mathf.Abs(revisionTraits.Vigilance.Value));

        //Not yet
        //reviseur.Creature.ColorSwap.Swap(CreatureTraits.GetColor(reviseur.Creature.Traits));
    }
}

[System.Serializable]
public class RevisionTraits {
    public RevisionTrait Aggressivity;
    public RevisionTrait Carnivorous;
    public RevisionTrait Constitution;
    public RevisionTrait Vigilance;
}

[System.Serializable]
public class RevisionTrait {
    [Range(-1, 1)]
    public float Value = 0;
}
