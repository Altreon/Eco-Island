using System;
using System.Collections.Generic;
using UnityEngine;

public enum CREATURE_GAUGES
{
    HUNGER, REPRODUCTION, LIFE
}

[System.Serializable]
public class CreatureGauges
{
    public CreatureGauges(CreatureGauges toCopy)
    {
		_hunger = new Gauge();
        _hunger.UpdateMax(toCopy._hunger.MaxSize);
        _hunger.Rate = toCopy._hunger.Rate;
        _hunger.ModifPerSecond = toCopy._hunger.ModifPerSecond;
		_reproduction = new Gauge();
        _reproduction.UpdateMax(toCopy._reproduction.MaxSize);
        _reproduction.Rate = toCopy._reproduction.Rate;
        _reproduction.ModifPerSecond = toCopy._reproduction.ModifPerSecond;
		_life = new Gauge();
        _life.UpdateMax(toCopy._life.MaxSize);
        _life.Rate = toCopy._life.Rate;
        _life.ModifPerSecond = toCopy._life.ModifPerSecond;

    }
    
    [Header("Please call UpdateValues in Update")]
    [SerializeField] private Gauge _hunger;
    public Gauge Hunger
    {
        get { return _hunger;}
        private set { _hunger = value; }
    }

    [SerializeField] private Gauge _reproduction;
    public Gauge Reproduction
    {
        get { return _reproduction;}
        private set { _reproduction = value; }
    }
    
    [SerializeField] private Gauge _life;
    public Gauge Life
    {
        get { return _life;}
        private set { _life = value; }
    }
    
    public Gauge Get(CREATURE_GAUGES value)
    {
        switch (value)
        {
            case CREATURE_GAUGES.HUNGER:
                return Hunger;
            case CREATURE_GAUGES.REPRODUCTION:
                return Reproduction;
            default:
                return Life;
        }
    }
    
    private float hungerToUpdate = 0f;
    private float reproToUpdate = 0f;
    private float lifeToUpdate = 0f;
    public void UpdateGauges(float deltaTime)
    {
        hungerToUpdate = updateGauge(hungerToUpdate, Hunger, deltaTime);
        reproToUpdate = updateGauge(reproToUpdate, Reproduction, deltaTime);
        lifeToUpdate = updateGauge(lifeToUpdate, Life, deltaTime);
    }

    private float updateGauge(float currentUpdate, Gauge gauge, float deltaTime)
    {
        if (Mathf.Abs(gauge.ModifPerSecond) >= float.Epsilon)
        {
            currentUpdate += gauge.ModifPerSecond * deltaTime;
            if (Mathf.Abs(currentUpdate) >= 1f)
            {
                int value = (int) currentUpdate;
                if(gauge + value < 0)
                {
                    gauge.Value = 0;
                    currentUpdate = 0f;
                }else if (gauge + value > gauge.MaxSize)
                {
                    gauge.Value = gauge.MaxSize;
                    currentUpdate = 0f;
                }
                else
                {
                    gauge.Value += value;
                    currentUpdate -= value;    
                }
            }
        }
        return currentUpdate;
    }
    
    public void InitializeGauges(CreatureTraits traits)
    {
        Debug.LogError("This function is not used anymore, please check GaugesConfig asset instead");
    }
}
