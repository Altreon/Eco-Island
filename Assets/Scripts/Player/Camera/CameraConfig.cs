using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CameraConfig", menuName = "Player/CameraConfig", order = 1)]
public class CameraConfig : ScriptableObject
{
    [Header("FPS")]
    public float fpsMinAngleY = -60.0f;
    public float fpsMaxAngleY = 10.0f;
    [Min(0f)]
    public float fpsHeightFromTarget = 1.73f;
    [Min(0f)]
    public float fpsSensitivityX = 0.004f;
    [Min(0f)]
    public float fpsSensitivityY = 0.001f;
    [Min(0f)]
    public float fpsSmooth = 0.1f;

    [Header("TPS")]
    public float tpsMinAngleY = -10.0f;
    public float tpsMaxAngleY = 60.0f;
    [Min(0f)]
    public float tpsDistance = 5.0f;
    public float tpsHeightFromTarget = 1f;
    [Min(0f)]
    public float tpsSensitivityX = 0.004f;
    [Min(0f)]
    public float tpsSensitivityY = 0.001f;
    [Min(0f)]
    public float tpsSmooth = 0.1f;
}
