using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

/**
Classe : PlayerSettings
Classe représentant la configuration graphique choisi par l'utilisateur de l'application
*/

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings Instance {get; private set;}

    private PostProcessLayer.Antialiasing antiAliasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
    public Action<PostProcessLayer.Antialiasing> AntiAliasingModeChanged;
    public PostProcessLayer.Antialiasing AntiAliasingMode {
        get{ return antiAliasingMode; }
        set{
            if(antiAliasingMode == value) return;

            antiAliasingMode = value;
            AntiAliasingModeChanged?.Invoke(value);
        }
    }
    public void IntToAntiAliasingMode (int value) {
        AntiAliasingMode = (PostProcessLayer.Antialiasing)value;
    }

    private bool ambiantOcclusion = true;
    public Action<bool> AmbiantOcclusionChanged;
    public bool AmbiantOcclusion {
        get{ return ambiantOcclusion; }
        set{
            if(ambiantOcclusion == value) return;

            ambiantOcclusion = value;
            AmbiantOcclusionChanged?.Invoke(value);
        }
    }

    private bool bloom = true;
    public Action<bool> BloomChanged;
    public bool Bloom {
        get{ return bloom; }
        set{
            if(bloom == value) return;

            bloom = value;
            BloomChanged?.Invoke(value);
        }
    }

    private bool fog = true;
    public Action<bool> FogChanged;
    public bool Fog {
        get{ return fog; }
        set{
            if(fog == value) return;

            fog = value;
            FogChanged?.Invoke(value);
        }
    }

    private float sensiX = 0.002f;
    public Action<float> SensiXChanged;
    public float SensiX {
        get{ return sensiX; }
        set{
            if(sensiX == value) return;

            sensiX = value;
            SensiXChanged?.Invoke(value);
        }
    }

    private float sensiY = 0.001f;
    public Action<float> SensiYChanged;
    public float SensiY {
        get{ return sensiY; }
        set{
            if(sensiY == value) return;

            sensiY = value;
            SensiYChanged?.Invoke(value);
        }
    }

    private bool meteo = true;
    public Action<bool> MeteoChanged;
    public bool Meteo {
        get{ return meteo; }
        set{
            if(meteo == value) return;

            meteo = value;
            MeteoChanged?.Invoke(value);
        }
    }
    
    private void Awake () {
        if(Instance) {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy () {
        if(Instance == this) Instance = null;
    }
}
