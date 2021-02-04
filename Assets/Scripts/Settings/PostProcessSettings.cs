using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessSettings : MonoBehaviour
{

    [SerializeField]
    private PostProcessVolume dayProfile = null;
    [SerializeField]
    private PostProcessVolume nightProfile = null;

    private void Start() {
        if(!PlayerSettings.Instance){
            Destroy(this);
            return;
        }

        PlayerSettings.Instance.AmbiantOcclusionChanged += AmbiantOcclusionChanged;
        PlayerSettings.Instance.BloomChanged += BloomChanged;
        PlayerSettings.Instance.FogChanged += FogChanged;

        AmbiantOcclusionChanged(PlayerSettings.Instance.AmbiantOcclusion);
        BloomChanged(PlayerSettings.Instance.Bloom);
        FogChanged(PlayerSettings.Instance.Fog);
    }

    private void OnDestroy() {
        if(PlayerSettings.Instance){
            PlayerSettings.Instance.AmbiantOcclusionChanged -= AmbiantOcclusionChanged;
            PlayerSettings.Instance.BloomChanged -= BloomChanged;
            PlayerSettings.Instance.FogChanged -= FogChanged;
        }

        AmbiantOcclusionChanged(true);
        BloomChanged(true);
        FogChanged(true);
    }

    private void AmbiantOcclusionChanged (bool value) {
        dayProfile.profile.GetSetting<AmbientOcclusion>().active = value;
        nightProfile.profile.GetSetting<AmbientOcclusion>().active = value;
    }

    private void BloomChanged (bool value) {
        dayProfile.profile.GetSetting<Bloom>().active = value;
        nightProfile.profile.GetSetting<Bloom>().active = value;
    }

    private void FogChanged (bool value) {
        dayProfile.profile.GetSetting<IL3DN_Fog_PP>().active = value;
        nightProfile.profile.GetSetting<IL3DN_Fog_PP>().active = value;
    }
}
