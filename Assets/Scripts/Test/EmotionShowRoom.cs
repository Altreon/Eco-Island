using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionShowRoom : MonoBehaviour
{
    public float speedRotation = 1;

    private Creature creature;
    private EmotionState emotion = 0;
    private float angle = 360;

    private bool emotionSet = false;

    private void Awake () {
        creature = GetComponent<Creature>();
    }

    private void Update () {
        angle += speedRotation * Time.deltaTime * ((Mathf.Abs(angle) % 360 > 250 || Mathf.Abs(angle) % 360 < 110) ? 7.5f : 1);
        transform.rotation = Quaternion.Euler(0, angle, 0);

        if(emotionSet){
            if(Mathf.Abs(angle) % 360 > 300){
                emotionSet = false;
            }
        }else{
            if(Mathf.Abs(angle) % 360 < 20) {
                ++emotion;
                if(emotion == EmotionState.Dying) ++ emotion;

                creature.FaceSwap.Swap(emotion);

                if(emotion == EmotionState.EnvyPacific) emotion = 0;
                emotionSet = true;
            }
        }
    }
}
