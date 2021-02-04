using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : SeasonManager
Système modifiant la saison de la simulation en fonction de l'avancement du cycle
*/

public class SeasonManager : MonoBehaviour
{
    private float terrainValueWinter = 0;
    private float pinesValueWinter = 0;
    private float leavesValueWinter = 0;
    private float branchesValueWinter = 0;
    private float rocksValueWinter = 0;
    private float grassValueWinter = 0;
    private float cutoffValueWinter = 1;

    private float transition = 0;
    private float smoothTransition = 0;

    public int whichSeason {get; private set;}

    public bool IsWinter()
    {
        return whichSeason % 2 == 1;
    }

    [Range(-30f, 50f)]
    [SerializeField] private float _tempMin = -5f;
    [Range(-30f, 50f)]
    [SerializeField] private float _tempMax = 15f;
    [Range(-30f, 50f)]
    [SerializeField] private float _tempValueEditor = 20f;
    [Min(0)]
    [SerializeField] private float _transitionSpeed = 1f;

    [SerializeField] private bool _byDayCycle = true;

    /*[Min(2)]
    [SerializeField] private int _transitionDay = 4;
    [Range(0.1f, 1f)]
    [SerializeField] private float _transitionPeriodStart = 0.3f;
    [Range(0f, 0.9f)]
    [SerializeField] private float _transitionPeriodEnd = 0.7f;*/

    [Min(2)]
    public int _transitionDay = 4;
    [Range(0.1f, 1f)]
    public float _transitionPeriodStart = 0.3f;
    [Range(0f, 0.9f)]
    public float _transitionPeriodEnd = 0.7f;

    //private bool hasInit = false;

    private float _tempValue = 20f;
    public float TempValue {
        get
        {
            return _tempValue;
        }
        set
        {
            _tempValue = value;
            _tempValueEditor = value;
            OnTempSet(_tempValue);
        }
    }

    private IL3DN.IL3DN_Snow IL3DN_SNOW;

    public static SeasonManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null && DayCycleManager.Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        IL3DN_SNOW = GetComponent<IL3DN.IL3DN_Snow>();
        terrainValueWinter = 1;
        pinesValueWinter = 4;
        leavesValueWinter = 1;
        branchesValueWinter = 1;
        rocksValueWinter = 1;
        grassValueWinter = 1;
        cutoffValueWinter = 2.1f;
        transition = 1;
    }

    private void Update()
    {
        if (_byDayCycle)
        {
            int whichDay = (int)DayCycleManager.Instance.Cycle % _transitionDay;
            whichSeason = (int)DayCycleManager.Instance.Cycle / _transitionDay;

            if(DayCycleManager.Instance.Cycle > 0.3f && DayCycleManager.Instance.Cycle % _transitionDay < 0.3f){
                whichSeason--;
            }

            if (whichDay == 0 && (int)DayCycleManager.Instance.Cycle != 0)
            {
                float actualDayProgression = DayCycleManager.Instance.Cycle - (int)DayCycleManager.Instance.Cycle;

                if (actualDayProgression > _transitionPeriodStart)
                {
                    IL3DN_SNOW.Snow = true;
                    transition = Mathf.Min((actualDayProgression - _transitionPeriodStart) / (_transitionPeriodEnd - _transitionPeriodStart), 1);
                    if (IsWinter())
                    {
                        transition = 1 - transition;
                    }
                }
            }
            else
            {
                if (IsWinter())
                {
                    IL3DN_SNOW.Snow = true;
                }
                else IL3DN_SNOW.Snow = false;
            }
        }


        if (_tempValueEditor != TempValue)
        {
            TempValue = _tempValueEditor;
        }

        UpdateTransition();
    }

    public void TempSet(float temp)
    {
        if (temp < -30f) TempValue = -30f;
        else if (temp > 50f) TempValue = 50f;
        else TempValue = temp;
    }

    private void OnTempSet(float temp)
    {
        if (temp <= _tempMax)
        {
            IL3DN_SNOW.Snow = true;
            if (temp < _tempMin)
            {
                transition = 0;
            }
            else transition = Mathf.Min((temp - _tempMin) / (_tempMax-_tempMin), 1);
        }
        else IL3DN_SNOW.Snow = false;
    }

    private void UpdateTransition () {
        smoothTransition = Mathf.Lerp(smoothTransition, transition, Time.deltaTime * _transitionSpeed);

        IL3DN_SNOW.SnowTerrain = Mathf.Lerp(terrainValueWinter, 0, smoothTransition);
        IL3DN_SNOW.SnowPines = Mathf.Lerp(pinesValueWinter, 0, smoothTransition);
        IL3DN_SNOW.SnowLeaves = Mathf.Lerp(leavesValueWinter, 0, smoothTransition);
        IL3DN_SNOW.SnowBranches = Mathf.Lerp(branchesValueWinter, 0, smoothTransition);
        IL3DN_SNOW.SnowRocks = Mathf.Lerp(rocksValueWinter, 0, smoothTransition);
        IL3DN_SNOW.SnowGrass = Mathf.Lerp(grassValueWinter, 0, smoothTransition);
        IL3DN_SNOW.CutoffLeaves = Mathf.Lerp(cutoffValueWinter, 1, smoothTransition);
    }
}
