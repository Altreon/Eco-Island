using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEditor;
using System;

/**
Classe : DayCycle
Système contrôllant le cycle jour/nuit en fonction du cycle courant de la simulation
*/

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class DayCycle : MonoBehaviour
{
    [SerializeField]
    private Light sun = null;
    [SerializeField]
    private Light moon = null;

    [SerializeField]
    private Material daySkyBox = null;
    [SerializeField]
    private Material nightSkyBox = null;
    [SerializeField]
    private Material cloudySkyBox = null;

    [SerializeField]
    private Material dayFog = null;
    [SerializeField]
    private Material nightFog = null;
    [SerializeField]
    private Material proceduralFog = null;

    [SerializeField]
    public PostProcessVolume dayVolume = null;
    [SerializeField]
    public PostProcessVolume nightVolume = null;

    private bool isNight = false;


    [Range(0.0f, 86400.0f)]
    public float Seconds = 34000;
    public bool isCloudy = false;

    private void Awake () {
        float blend = Math.Abs((float)this.Seconds/43200.0f-1.0f);
        isNight = blend > 0.5f;
    }

    private void Update() {
        float blend = Math.Abs((float)this.Seconds/43200.0f-1.0f);
        //float lossyBlend = Mathf.Sin((Mathf.PI / 2) * (float)(Math.Pow(blend, 0.5f)));

        Vector3 position =
            Quaternion.Euler(((float)this.Seconds/86400.0f)*360.0f, 0, 0) *
            new Vector3(0.0f, -300.0f, 0.0f);
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(-position);


        /*if(blend < 0.5){
            sun.intensity = (0.5f - blend) * 2;
            Debug.Log(sun.intensity);
            moon.intensity = 0;
        }else{
            sun.intensity = 0;
            moon.intensity = (1/3f) * blend / 0.5f;
        }*/

        //sun.intensity = 1.25f - Mathf.Sin((Mathf.PI / 2) * blend);
        //moon.intensity = (1/3f) - (1 - blend) / 3;
        /*sun.color = new Color(
            1.0f,
            Math.Min(sun.intensity + 0.05f, 1.0f),
            Math.Min(sun.intensity, 1.0f)
        );*/
        
        dayVolume.weight = 1 - blend;
        nightVolume.weight = blend;

        //dayVolume.profile.GetSetting<IL3DN_Fog_PP>()._Density.value = 0.5f * (1 - blend);
        //nightVolume.profile.GetSetting<IL3DN_Fog_PP>()._Density.value = 0.5f * blend;

        Material day = isCloudy ? cloudySkyBox : daySkyBox;

        RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(day.GetColor("_SkyTint"), nightSkyBox.GetColor("_SkyTint"), blend));
        RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(day.GetFloat("_Exposure"), nightSkyBox.GetFloat("_Exposure"), blend));

        proceduralFog.SetColor("_NearColor", Color.Lerp(dayFog.GetColor("_NearColor"), nightFog.GetColor("_NearColor"), blend));
        proceduralFog.SetColor("_FarColor", Color.Lerp(dayFog.GetColor("_FarColor"), nightFog.GetColor("_FarColor"), blend));
        proceduralFog.SetColor("_GlowColor", Color.Lerp(dayFog.GetColor("_GlowColor"), nightFog.GetColor("_GlowColor"), blend));

        if(!isNight && blend > 0.5f){
            RenderSettings.sun = moon;
        }else if(isNight && blend <= 0.5f){
            RenderSettings.sun = sun;
        }
        isNight = blend > 0.5f;

        //RenderSettings.skybox.SetFloat("_Blend", blend);
        //RenderSettings.skybox.SetFloat("_Rotation", ((float)this.Seconds/86400.0f)*360.0f);
    }
}
