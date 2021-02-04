using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoppedScript : MonoBehaviour
{
    [SerializeField] private ParticleType myPartType;
    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        ParticuleManager.Instance.DestroyParticle(myPartType, gameObject);
        //Debug.Log("stoppé");
    }
}