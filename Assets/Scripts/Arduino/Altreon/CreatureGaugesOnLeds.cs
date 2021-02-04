using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGaugesOnLeds : MonoBehaviour
{
    [SerializeField]
    private SerialHandler serialHandler = null;
    [SerializeField]
    private float updateFreq = 0.1f;
    
    public Creature Observed = null;


    private void Awake() {
        if(!serialHandler){
            enabled = false;
        }
    }

    private void Start () {
        StartCoroutine(UpdateLeds());
        /*serialHandler.SetLed(1, 0);
        serialHandler.SetLed(2, 0);
        serialHandler.SetLed(3, 0);*/
    }

    private IEnumerator UpdateLeds () {
        while(true){
            yield return new WaitForSeconds(updateFreq);

            if(Observed != null){
                serialHandler.SetLed(1, Observed.Gauges.Hunger.Rate);
                serialHandler.SetLed(2, Observed.Gauges.Reproduction.Rate);
                serialHandler.SetLed(3, Observed.Gauges.Life.Rate);
            }else{
                serialHandler.SetLed(1, 0);
                serialHandler.SetLed(2, 0);
                serialHandler.SetLed(3, 0);
            }
        }
    }  
}
