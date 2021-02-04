using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWeatherByArduino : MonoBehaviour
{
    [SerializeField]
    private SerialHandler serialHandler = null;

    public GameObject RainStorn;
    public GameObject SnowStorn;

    private ParticleSystem RainStornPS;
    private ParticleSystem SnowStornPS;

    public enum WeatherState
    {
        Rain,
        Snow
    }
    
    public WeatherState weatherState;

    private float ValuePotentiometre;
    private float ClampValue;

    private void Start() 
    {
        DayCycleManager.Instance.IsCloudy = true;
        if(weatherState == WeatherState.Rain)
        {
            SnowStorn.SetActive(false);
            RainStorn.SetActive(true);                
            RainStornPS = GetComponentInChildren<ParticleSystem>();
        }
        else
        {
            RainStorn.SetActive(false);
            SnowStorn.SetActive(true);
            SnowStornPS = GetComponentInChildren<ParticleSystem>();
        }
    }

    private void Update()
    {
        ValuePotentiometre = serialHandler.ValuePotentiometre;
        ClampValue = ValuePotentiometre / 1023f;

        if(weatherState == WeatherState.Rain)
        {
            var emission = RainStornPS.emission;
            var vel = RainStornPS.velocityOverLifetime;
            
            emission.rateOverTime = 7000 * ClampValue;
            vel.x = 8*ClampValue;
            vel.speedModifier = ClampValue + 1;          
        }
        else
        {
            var emission = SnowStornPS.emission;
            var vel = SnowStornPS.velocityOverLifetime;
            
            emission.rateOverTime = 2000 * ClampValue;
            //vel.x = 5*ClampValue;
            vel.speedModifier = ClampValue; 
        }
    }
}
