using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/**
Classe : CameraController
Gestion de la position et de la rotation de la caméra dans le monde du jeu
*/

public class CameraController : MonoBehaviour
{
    public CameraConfig CameraPreset;

    static CameraController instance = null;
    [SerializeField] private FlashlightController flashlightController = null;
    public FlashlightController FlashlightController {get{return flashlightController;}}

    public GameObject Target;
    public float ViewPadFactor = 10;

    private float fpsMinAngleY;
    private float fpsMaxAngleY;
    private float fpsHeightFromTarget;
    private float fpsSensitivityX;
    private float fpsSensitivityY;
    private float fpsSmooth;
    private float TargetX { get; set; } = 0.0f;
    private float TargetY { get; set; } = 0.0f;
    private float CurrentX { get; set; } = 0.0f;
    private float CurrentY { get; set; } = 0.0f;

    private Vector3 direction;
    private Quaternion rotation;

    private Vector2 dir;

    public bool CanControll {get; set;} = true;
    public float SensitivityX {get{return fpsSensitivityX;} set{fpsSensitivityX = value;}}
    public float SensitivityY {get{return fpsSensitivityY;} set{fpsSensitivityY = value;}}

    private void Awake () {
        if(instance){
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        SetupPreset();
    }

    private void SetupPreset()
    {
        fpsMinAngleY = CameraPreset.fpsMinAngleY;
        fpsMaxAngleY = CameraPreset.fpsMaxAngleY;
        fpsHeightFromTarget = CameraPreset.fpsHeightFromTarget;
        fpsSensitivityX = CameraPreset.fpsSensitivityX;
        fpsSensitivityY = CameraPreset.fpsSensitivityY;
        fpsSmooth = CameraPreset.fpsSmooth;       
    }

    public static CameraController Instance 
    {
        get 
        {
            return instance;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void LookMouse () {
        dir = Mouse.current.delta.ReadValue();
    }

    private void LookJoystick () {
        Vector2 move = Gamepad.current.rightStick.ReadValue(); 
        dir =  (move.magnitude < 0.05f ? Vector2.zero : move * ViewPadFactor);
    }

    private void Update()
    {
        if(!Target)
        {
            Debug.LogWarning("Camera doesn't have target object");
            return;
        }

        if(CanControll) {
            if(Mouse.current != null){
                LookMouse();
            }
            if(Gamepad.current != null && dir == Vector2.zero){
                LookJoystick();
            }
        }else{
            dir = Vector2.zero;
        }

        TargetX += -dir.x * fpsSensitivityX;
        TargetY += -dir.y * fpsSensitivityY * -1;

        float smooth = fpsSmooth;

        CurrentX = Mathf.Lerp(CurrentX, TargetX, Time.deltaTime / smooth);
        CurrentY = Mathf.Lerp(CurrentY, TargetY, Time.deltaTime / smooth);

        TargetY = Mathf.Clamp(TargetY, fpsMinAngleY  * Mathf.Deg2Rad, fpsMaxAngleY  * Mathf.Deg2Rad);


        FpsUpdate();
    }

    private void FpsUpdate () 
    {
        transform.position = FpsTargetPos();

        Vector3 viewDir = ViewOnFpsDir();
        transform.LookAt(viewDir);
    }

    private Vector3 ViewOnFpsDir()
    {
        float theta = CurrentX;
        float x = Mathf.Cos(theta);
        float z = Mathf.Sin(theta);

        theta = CurrentY;
        
        float y = Mathf.Sin(theta);

        Vector3 pos = new Vector3(x, y, z);

        return FpsTargetPos() + pos;
    }

    private Vector3 FpsTargetPos () {
        return Target.transform.position + Vector3.up * fpsHeightFromTarget;
    }
}
