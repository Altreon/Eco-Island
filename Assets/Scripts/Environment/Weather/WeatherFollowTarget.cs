using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherFollowTarget : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset;

    private void Update() {
        if(!Target) return;

        transform.position = new Vector3(Target.position.x + offset.x, Target.position.y + offset.y, Target.position.z + offset.z);
    }
}