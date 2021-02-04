using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DayCycle))]
public class ControlDayCycleByArduino : MonoBehaviour
{
    [SerializeField]
    private SerialHandler serialHandler = null;
    public float changeSpeed = 100f;

    private float targetSecond = 43200;
    private float second = 43200;

    private DayCycle dayCycle;
    private void Awake() {
        dayCycle = GetComponent<DayCycle>();

        if(!serialHandler){
            enabled = false;
        }
    }

    private void Start () {
        serialHandler.Swap += Swap;
    }

    private void OnDestroy () {
        serialHandler.Swap -= Swap;
    }

    private void Swap (bool isRight){
        targetSecond = isRight ? targetSecond + 43200 : targetSecond - 43200;
        /*if(targetSecond < 0){
            targetSecond = 86400;
        }else if(targetSecond > 86400){
            targetSecond = 0;
        }*/
    }

    /*private void Update()
    {
        float value = Mathf.Lerp(0, 86400, serialHandler.Distance / 50);
        dayCycle.Seconds = smoothy ? Mathf.Lerp(dayCycle.Seconds, value, 10f * Time.deltaTime) : value;
    }*/

    private void Update () {
        second = Mathf.Lerp(second, targetSecond, changeSpeed * Time.deltaTime);
        dayCycle.Seconds = Mathf.Abs(second % 86400);
    }
}
