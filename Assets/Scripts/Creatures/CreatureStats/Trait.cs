using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class Trait
{
     
    public Trait(CreatureTraits creature = null)
    {
        creatureToNotify = creature;
    }
    [System.NonSerialized] private CreatureTraits creatureToNotify = null; //Très important de non serialize, sinon serialization infinie
    [SerializeField] [Range(0f,1f)] private float _value;

    public float Value
    {
        get { return _value;}
        set{
            Assert.IsTrue(value >= 0f && value <= 1f, "This trait can't be set to " + value + " because it needs to be 0 and 1");
            _value = value;
            if (creatureToNotify != null)
            {
                creatureToNotify.notifyTraitChanged(this);
            }
        }
    }

    /// <summary>
    /// Updates the value of this traits staying in the [0 ; 1] interval
    /// </summary>
    /// <param name="newValue">The new value : if it is > 1f, then will put the trait to 1f. If it is < 0f, then will put the trait to 0f</param>
    public void UpdateValueClamped(float newValue)
    {
        if (newValue > 1f) 
            newValue = 1f;
        else if (newValue < 0f)
            newValue = 0f;
        Value = newValue;
    }

    /// <see cref="UpdateValueClamped"/>
    /// <summary>trait.AddValueClamped(0.1f); <=> trait.UpdateValueClamped(trait.Value + 0.1f);</summary>
    public void AddValueClamped(float toAdd)
    {
        UpdateValueClamped(Value + toAdd);
    }
    
    public static implicit operator float(Trait a)
    {
        return a._value;
    }
/* BON OK ALTREON ON FAIT PAS CA :'(
    public static implicit operator Trait(float a)
    {
        return new Trait(a);
    }
    */
}
