using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCreatureOffset : MonoBehaviour
{
    public Vector3 offset16_9;
    public Vector3 offset16_10;
    public Vector3 offset5_4;
    public Vector3 offset4_3;

    private void Update () {
        float ratio = (float)Screen.width / Screen.height;
        float e = 0.01f;

        if(ratio >= 4f/3 - e && ratio <= 4f/3 + e){
            transform.position = offset4_3;
        }else if(ratio >= 5f/4 - e && ratio <= 5f/4 + e){
            transform.position = offset5_4;
        }else if(ratio >= 16f/10 - e && ratio <= 16f/10 + e){
            transform.position = offset16_10;
        }else {
            transform.position = offset16_9;
        }
    }
}
