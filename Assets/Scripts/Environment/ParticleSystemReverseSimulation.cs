using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ParticleSystemReverseSimulation : MonoBehaviour
{
    public ParticleSystem[] particleSystems;

    private float simulationTime;

    private void Start () {
        for (int i = 0; i < particleSystems.Length; i++) {
            //particleSystems[i].Simulate(1, false, true);
        }

        simulationTime = 1;
    }

    private void FixedUpdate () {
        simulationTime -= Time.fixedDeltaTime * 0.5f;
        if(simulationTime < 0) {
            simulationTime = 1 - Time.fixedDeltaTime;
        }

        for (int i = 0; i < particleSystems.Length; i++) {
            particleSystems[i].Simulate(simulationTime, false, true);
        }
    }
}
