using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraSettings : MonoBehaviour
{
    private PostProcessLayer layer;
    private CameraController controller;
    private IL3DN.IL3DN_Fog fog;

    private void Awake () {
        layer = GetComponent<PostProcessLayer>();
        fog = GetComponent<IL3DN.IL3DN_Fog>();
        controller = GetComponent<CameraController>();
    }

    private void Start() {
        if(!PlayerSettings.Instance){
            Destroy(this);
            return;
        }

        PlayerSettings.Instance.AntiAliasingModeChanged += AntiAliasingModeChanged;
        PlayerSettings.Instance.FogChanged += FogChanged;

        if(controller){
            PlayerSettings.Instance.SensiXChanged += SensiXChanged;
            PlayerSettings.Instance.SensiYChanged += SensiYChanged;
        }

        AntiAliasingModeChanged(PlayerSettings.Instance.AntiAliasingMode);
        FogChanged(PlayerSettings.Instance.Fog);

        if(controller){
            SensiXChanged(PlayerSettings.Instance.SensiX);
            SensiYChanged(PlayerSettings.Instance.SensiY);
        }
    }

    private void OnDestroy() {
        if(PlayerSettings.Instance){
            PlayerSettings.Instance.AntiAliasingModeChanged -= AntiAliasingModeChanged;
            PlayerSettings.Instance.FogChanged -= FogChanged;

            if(controller){
                PlayerSettings.Instance.SensiXChanged -= SensiXChanged;
                PlayerSettings.Instance.SensiYChanged -= SensiYChanged;
            }
        }
    }

    private void AntiAliasingModeChanged (PostProcessLayer.Antialiasing value) {
        layer.antialiasingMode = value;
    }

    private void FogChanged (bool value) {
        fog.enabled = value;
    }

    private void SensiXChanged (float value) {
        controller.SensitivityX = value;
    }

    private void SensiYChanged (float value) {
        controller.SensitivityY = value;
    }
}
