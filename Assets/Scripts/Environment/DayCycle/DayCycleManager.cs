using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : DayCycleManager
Système prenant en charge l'avancement du cycle de la simulation
*/

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance {get; private set;}

    [SerializeField]
    public DayCycle dayCycle = null;
    [Range(0, 10000)]
    public float speedByRealtime = 1;
    
    [SerializeField]
    private float DebugCycle;

    public float Cycle {get; private set;}
    public bool IsCloudy {
        get{
            return dayCycle ? dayCycle.isCloudy : false;
        }
        set{
            if(dayCycle) dayCycle.isCloudy = value;
        }
    }

    private void Awake() {
        if(Instance){
            Destroy(this);
            return;
        }

        Instance = this;

        if(!dayCycle){
            enabled = false;
        }
    }

    private void OnDestroy () {
        if(Instance == this) Instance = null;
    }

    private void Update()
    {
        float t = Time.deltaTime * speedByRealtime;
        float s = dayCycle.Seconds + t;

        if(s > 86400) {
            s = s - 86400;
        }

        dayCycle.Seconds = s;

        Cycle += t / 86400;

        DebugCycle = Cycle;
    }
}
