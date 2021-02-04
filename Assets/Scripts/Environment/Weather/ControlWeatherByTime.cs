using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : ControlWeatherByTime
Système de contrôles des intempéries, en fonction du cycle de la simulation
*/

public class ControlWeatherByTime : MonoBehaviour
{ 
    [SerializeField] private ParticleSystem RainStormPS = null;
    [SerializeField] private ParticleSystem SnowStormPS = null;

    /*[Range(0f, 1.5f)]
    [SerializeField]
    private float ClampValue = 0f;*/

    /*[Min(1)]
    [SerializeField]
    private int rarity = 10;

    [Range(1, 10)]
    [SerializeField]
    private int maxDuration = 5;*/

    [Range(0.1f, 0.45f)]
    [SerializeField]
    private float minDuration = 0.3f;

    [Range(0.55f, 1f)]
    [SerializeField]
    private float maxDuration = 0.6f;

    [Min(0)]
    [SerializeField] private float transitionCycle = 0.05f;

    [Range(0.75f, 1.25f)]
    [SerializeField] private float minRainCoolDown = 0.75f;
    [Range(1.75f, 4.5f)]
    [SerializeField] private float maxRainCoolDown = 3.25f;

    //private float coolDown = 0;

    /*private int whichPeriod;
    private int lastPeriod;

    private int endPeriod;*/

    //private float whichPeriod = 0f;
    //private float endPeriod = 0f;
    private bool hasInit = false;

    private float nextWeatherStart;
    private float nextWeatherDuration;
    private float nextWeatherIntensity;

    private float oldWeatherEnd;
    private float oldWeatherIntensity;

    public bool IsRain {get; private set;}

    [SerializeField]
    private SoundLoop RainSound = SoundLoop.AmbianceRain;
    [SerializeField]
    private SoundLoop SnowSound = SoundLoop.AmbianceSnow;

    private AudioBox audioBox;

    private void Start()
    {
        audioBox = GetComponent<AudioBox>();
        //audioBox.PlayLoop(RainSound);
        DayCycleManager.Instance.IsCloudy = false;
        //coolDown = Random.Range(minRainCoolDown, maxRainCoolDown);
        //Debug.Log("Cooldown before restart: " + (endPeriod + coolDown));
        //ClampValue = 0f;

        if (PlayerSettings.Instance != null) PlayerSettings.Instance.MeteoChanged += MeteoChanged;
    }

    private void OnDestroy () {
        if (PlayerSettings.Instance != null) PlayerSettings.Instance.MeteoChanged -= MeteoChanged;
    }

    private void MeteoChanged (bool value){
        float cycle = DayCycleManager.Instance.Cycle;

        if(IsRain){
            if (SeasonManager.Instance.IsWinter()) {
                SnowStormPS.gameObject.SetActive(value);
                if(value){
                    audioBox.PlayLoop(SoundLoop.AmbianceSnow);
                }else{
                    audioBox.StopLoop(SoundLoop.AmbianceSnow);
                }
            }else{
                RainStormPS.gameObject.SetActive(value);
                if(value){
                    audioBox.PlayLoop(SoundLoop.AmbianceRain);
                }else{
                    audioBox.StopLoop(SoundLoop.AmbianceRain);
                }
            }
        }else{
            if (SeasonManager.Instance.IsWinter()) {
                if(SnowStormPS.gameObject.activeSelf && SnowStormPS.isPlaying){
                     SnowStormPS.gameObject.SetActive(value);
                    if(value){
                        audioBox.PlayLoop(SoundLoop.AmbianceSnow);
                    }else{
                        audioBox.StopLoop(SoundLoop.AmbianceSnow);
                    }
                }
            }else{
                if(SnowStormPS.gameObject.activeSelf && RainStormPS.isPlaying){
                    RainStormPS.gameObject.SetActive(value);
                    if(value){
                        audioBox.PlayLoop(SoundLoop.AmbianceRain);
                    }else{
                        audioBox.StopLoop(SoundLoop.AmbianceRain);
                    }
                }
            }
        }
    }

    private void Update()
    {
        //whichPeriod = DayCycleManager.Instance.Cycle;
        float cycle = DayCycleManager.Instance.Cycle;
        if (!hasInit)
        {
            Random.InitState(new System.Random().Next());
            /*coolDown = Random.Range(minRainCoolDown, maxRainCoolDown);
            Debug.Log("Cooldown before restart: " + (endPeriod + coolDown));*/
            NextWeather();
            hasInit = true;
        }
        /*if (whichPeriod >= endPeriod)
        {
            if (whichPeriod >= endPeriod + coolDown)
            {
                endPeriod = whichPeriod + Random.Range(minDuration, maxDuration);
                coolDown = Random.Range(minRainCoolDown, maxRainCoolDown);
                
                Debug.Log("EndPeriod: " + endPeriod);
                Debug.Log("Cooldown before restart: " + (endPeriod + coolDown));
                ClampValue = Random.Range(0.5f, 1.5f);
                DayCycleManager.Instance.IsCloudy = true;
                Debug.Log("ClampValue: " + ClampValue);
            }
            else
            {
                DayCycleManager.Instance.IsCloudy = false;
                ClampValue = 0f;
            }
        }
        smoothValueTransition = Mathf.Lerp(smoothValueTransition, ClampValue, Time.deltaTime * transitionSpeed);
        ChangeWeather();*/

        if(IsRain){
            if(cycle > nextWeatherStart + nextWeatherDuration){
                StopWeather();
                NextWeather();
            }else{
                UpdateInWeather();
            }
        }else{
            if(cycle > nextWeatherStart){
                StartWeather();
            }
            UpdateOutWeather();
        }
    }

    private void NextWeather () {
        float cycle = DayCycleManager.Instance.Cycle;
        SeasonManager season = SeasonManager.Instance;

        nextWeatherStart = cycle + Random.Range(minRainCoolDown, maxRainCoolDown);
        nextWeatherDuration = Random.Range(minDuration, maxDuration);
        nextWeatherIntensity = Random.Range(0.5f, 1.5f);

        float nextTransition = season._transitionDay * (season.whichSeason + 1) + season._transitionPeriodStart;

        if(nextWeatherStart + nextWeatherDuration + transitionCycle > nextTransition){
            nextWeatherStart = nextTransition;
            nextWeatherDuration = season._transitionPeriodEnd - season._transitionPeriodStart;
            nextWeatherIntensity = 1.5f;

            /*if(season.IsWinter()){

            }else{

            }*/
        }
    }

    private void PlayWeatherSound()
    {
        if (SeasonManager.Instance.IsWinter()) audioBox.PlayLoop(SnowSound); 
        else audioBox.PlayLoop(RainSound);
    }

    private void StopWeatherSound()
    {
        audioBox.StopLoop(SnowSound);
        audioBox.StopLoop(RainSound);
    }

    private void StartWeather () {
        IsRain = true;
        DayCycleManager.Instance.IsCloudy = true;
        if(PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo) PlayWeatherSound();
    }

    private void StopWeather () {
        IsRain = false;
        DayCycleManager.Instance.IsCloudy = false;
        //StopWeatherSound();

        oldWeatherEnd = nextWeatherStart + nextWeatherDuration;
        oldWeatherIntensity = nextWeatherIntensity;
    }

    private void UpdateInWeather()
    {
        float cycle = DayCycleManager.Instance.Cycle;
        float smoothValueTransition = Mathf.Lerp(0, nextWeatherIntensity, (cycle - nextWeatherStart) / transitionCycle);

        if (SeasonManager.Instance.IsWinter())
        {
            SnowStormPS.gameObject.SetActive(PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo);
            if (!SnowStormPS.isPlaying && (PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo)) SnowStormPS.Play();

            var emission = SnowStormPS.emission;
            var vel = SnowStormPS.velocityOverLifetime;

            if(PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo) audioBox.SetLoopVolume(SnowSound, Mathf.Clamp((cycle - nextWeatherStart) / transitionCycle, 0, 1));
            emission.rateOverTime = 2000 * smoothValueTransition;
            vel.speedModifier = smoothValueTransition;
        }
        else
        {
            RainStormPS.gameObject.SetActive(PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo);
            if (!RainStormPS.isPlaying && (PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo)) RainStormPS.Play();

            var emission = RainStormPS.emission;
            var vel = RainStormPS.velocityOverLifetime;

            if (PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo) audioBox.SetLoopVolume(RainSound, Mathf.Clamp((cycle - nextWeatherStart) / transitionCycle, 0, 1));
            emission.rateOverTime = 7000 * smoothValueTransition;
            vel.x = 8 * smoothValueTransition;
            vel.speedModifier = smoothValueTransition + 1;
        }
    }

    private void UpdateOutWeather()
    {
        float cycle = DayCycleManager.Instance.Cycle;
        float smoothValueTransition = Mathf.Lerp(oldWeatherIntensity, 0, (cycle - oldWeatherEnd) / transitionCycle);

        if (SeasonManager.Instance.IsWinter())
        {
            if (smoothValueTransition <= 0.1f && SnowStormPS.isPlaying){
                SnowStormPS.Stop();
                audioBox.StopLoop(SnowSound);
            }
            else if(SnowStormPS.isPlaying && (PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo)) audioBox.SetLoopVolume(SnowSound, 1 - Mathf.Clamp((cycle - oldWeatherEnd) / transitionCycle, 0, 1));

            if (smoothValueTransition < 0.01f)
            {
                SnowStormPS.gameObject.SetActive(false);
            }

            var emission = SnowStormPS.emission;
            var vel = SnowStormPS.velocityOverLifetime;


            emission.rateOverTime = 2000 * smoothValueTransition;
            vel.speedModifier = smoothValueTransition;
        }
        else
        {
            if (smoothValueTransition <= 0.1f && RainStormPS.isPlaying){
                RainStormPS.Stop();
                audioBox.StopLoop(RainSound);
            }
            else if(RainStormPS.isPlaying && (PlayerSettings.Instance == null || PlayerSettings.Instance.Meteo)) audioBox.SetLoopVolume(RainSound, 1 - Mathf.Clamp((cycle - oldWeatherEnd) / transitionCycle, 0, 1));

            if (smoothValueTransition < 0.01f)
            {
                RainStormPS.gameObject.SetActive(false);
            }

            var emission = RainStormPS.emission;
            var vel = RainStormPS.velocityOverLifetime;

            emission.rateOverTime = 7000 * smoothValueTransition;
            vel.x = 8 * smoothValueTransition;
            vel.speedModifier = smoothValueTransition + 1;
        }
    }
}
