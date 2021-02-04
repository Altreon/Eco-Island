using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public enum GAUGE_VALUE
{
    MODIFIER, MAXVALUE, RATE
}
[System.Serializable]
public class GaugeOperation
{
    //[Range(0f, 1f)]
    //public float traitInfluence;
    public CREATURE_TRAITS influence;
    public CREATURE_GAUGES influenced;
    public GAUGE_VALUE valueChanges;
    public float minValue;
    public float maxValue;
}
[CreateAssetMenu(fileName = "GaugesConfig", menuName = "Creatures/Gauge Initializer", order = 5)]
public class GaugesConfig : ScriptableObject
{
    public GaugeOperation[] modifiers;

    public void InitializeGauges(Creature toInitialize = null, bool initilizeRate = true, bool keepRate = true, bool test = false, CreatureTraits testTraits = null,
        CreatureGauges testGauges = null)
    {
        if (!test && toInitialize == null)
        {
            Debug.LogError("Warning ! No creature has been passed ! Aborting !");
            return;
        }

        CreatureTraits traits = null;
        if (!test)
            traits = toInitialize.Traits;
        else
            traits = testTraits;
        if (traits == null)
        {
            Debug.LogError("Tests are null ! Aborting");
            return;
        }

        if (test && testGauges == null)
        {
            Debug.LogError("Can't produce test without testGauges, Aborting !");
            return;
        }

        Dictionary<CREATURE_GAUGES, Dictionary<GAUGE_VALUE, float>> values =
            new Dictionary<CREATURE_GAUGES, Dictionary<GAUGE_VALUE, float>>();
        foreach (CREATURE_GAUGES TYPE_GAUGE in (CREATURE_GAUGES[]) System.Enum.GetValues(typeof(CREATURE_GAUGES)))
        {
            values.Add(TYPE_GAUGE, new Dictionary<GAUGE_VALUE, float>());
            foreach (GAUGE_VALUE VAL in (GAUGE_VALUE[]) System.Enum.GetValues(typeof(GAUGE_VALUE)))
            {
                values[TYPE_GAUGE].Add(VAL, 0f);
            }
        }

        foreach (GaugeOperation op in modifiers)
        {
            if (op.valueChanges == GAUGE_VALUE.RATE && initilizeRate)
            {
                if (test)
                {
                    values[op.influenced][GAUGE_VALUE.RATE] = Mathf.Lerp(op.minValue, op.maxValue, traits.Get(op.influence));
                }
                else
                {
                    values[op.influenced][GAUGE_VALUE.RATE] = UnityEngine.Random.Range(op.minValue, op.maxValue);
                }
            }
            else
            {
                values[op.influenced][op.valueChanges] +=
                    Mathf.Lerp(op.minValue, op.maxValue, traits.Get(op.influence));
            }
        }

        foreach (KeyValuePair<CREATURE_GAUGES, Dictionary<GAUGE_VALUE, float>> value in values)
        {
            foreach (KeyValuePair<GAUGE_VALUE, float> f in value.Value)
            {
                switch (f.Key)
                {
                    case GAUGE_VALUE.RATE:
                        if (!initilizeRate)
                            break;
                        if (!test)
                            toInitialize.Gauges.Get(value.Key).Rate = f.Value;
                        else
                            testGauges.Get(value.Key).Rate = f.Value;
                        break;
                    case GAUGE_VALUE.MAXVALUE:
                        if (!test)
                            toInitialize.Gauges.Get(value.Key).UpdateMax((int) f.Value, keepRate);
                        else
                            testGauges.Get(value.Key).UpdateMax((int) f.Value, keepRate);
                        break;
                    case GAUGE_VALUE.MODIFIER:
                        if (!test)
                            toInitialize.Gauges.Get(value.Key).ModifPerSecond = f.Value;
                        else
                            testGauges.Get(value.Key).ModifPerSecond = f.Value;
                        break;
                    default:
                        Debug.LogError("This isn't supposed to happen. Please update this function");
                        break;
                }
            }
        }
    }
#if UNITY_EDITOR
    [Header("Tester - Modify here")]
    public CreatureTraits testTraits;

    [Header("Watch here")] public Color colorCreature;
    public CreatureGauges testGauges;

    private float[] oldTraits;
    private bool oldInit = false;
    private void OnEnable()
    {
        initialized = true;
    }

    private void OnDisable()
    {
        initialized = false;
        oldInit = false;
    }

    public void resetOldTraits()
    {
        oldInit = true;
        oldTraits = new float[System.Enum.GetValues(typeof(CREATURE_TRAITS)).Length];//new CreatureTraits(testTraits);
        int i = 0;
        foreach (CREATURE_TRAITS tr in (CREATURE_TRAITS[]) System.Enum.GetValues(typeof(CREATURE_TRAITS)))
        {
            oldTraits[i] = testTraits.Get(tr).Value;
            ++i;
        }
    }

    private bool initialized = false;
    
    private void OnValidate()
    {
        if(initialized){
            if(!oldInit){
                resetOldTraits();
                return;
            }

            string toRecalculate = "";
            bool recalculate = false;
            int i = 0;
            foreach (CREATURE_TRAITS tr in (CREATURE_TRAITS[]) System.Enum.GetValues(typeof(CREATURE_TRAITS)))
            {
                recalculate = Mathf.Abs(oldTraits[i] - testTraits.Get(tr).Value) > 0.001f;
                toRecalculate = tr.ToString() +" set to " + testTraits.Get(tr).Value.ToString();
                ++i;
                if (recalculate)
                    break;
            }

            if (!recalculate)
                return;
            Debug.Log("Recalculating gauges with trait " + toRecalculate);
            colorCreature = CreatureTraits.GetColor(testTraits);
            InitializeGauges(null, true, true, true, testTraits, testGauges);
            resetOldTraits();
        }
    }
#endif

}
 