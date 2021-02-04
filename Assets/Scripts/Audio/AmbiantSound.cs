using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : AmbiantSound
Gère la musique d'ambience du jeu selon le cycle actuel de la simulation (jour ou nuit)
*/

public class AmbiantSound : MonoBehaviour
{
    [SerializeField]
    private SoundLoop dayAmbianceSound = SoundLoop.AmbianceDay;
    [SerializeField]
    private SoundLoop nightAmbianceSound = SoundLoop.AmbianceNight;

    private AudioBox audioBox;
    private bool isDay = true;

    private void Start () {
        audioBox = GetComponent<AudioBox>();
        audioBox.PlayLoop(dayAmbianceSound);
    }

    private void Update () {
        if(isDay && DayCycleManager.Instance.Cycle % 1 > 0.25f && DayCycleManager.Instance.Cycle % 1 < 0.75f){
            audioBox.StopLoop(dayAmbianceSound);
            audioBox.PlayLoop(nightAmbianceSound);

            isDay = false;
        }else if(!isDay && DayCycleManager.Instance.Cycle % 1 > 0.75f){
            audioBox.StopLoop(nightAmbianceSound);
            audioBox.PlayLoop(dayAmbianceSound);

            isDay = true;
        }
    }
}
