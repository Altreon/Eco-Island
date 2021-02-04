using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FacesTextures", menuName = "Estethics/FacesTextures", order = 0)]
public class FacesTextures : ScriptableObject
{
    [SerializeField]
    private Texture2D defaultFace = null;
    [SerializeField]
    private Texture2D happyFace = null;
    [SerializeField]
    private Texture2D hungryFace = null;
    [SerializeField]
    private Texture2D dyingFace = null;
    [SerializeField]
    private Texture2D scaredFace = null;
    [SerializeField]
    private Texture2D agressiveFace = null;
    [SerializeField]
    private Texture2D angryFace = null;
    [SerializeField]
    private Texture2D sleepFace = null;
    [SerializeField]
    private Texture2D suspiciousFace = null;
    [SerializeField]
    private Texture2D curiousFace = null;
    [SerializeField]
    private Texture2D loveFace = null;
    [SerializeField]
    private Texture2D tiredFace = null;
    [SerializeField]
    private Texture2D stunnedFace = null;
    [SerializeField]
    private Texture2D swallowFace = null;
    [SerializeField]
    private Texture2D envyFace = null;
    [SerializeField]
    private Texture2D friendlyFace = null;
    [SerializeField]
    private Texture2D envyPacificFace = null;

    public Texture2D GetFace (EmotionState emotion){
        switch (emotion){
            case EmotionState.Default : return defaultFace;
            case EmotionState.Happy : return happyFace;
            case EmotionState.Hungry : return hungryFace;
            case EmotionState.Dying : return dyingFace;
            case EmotionState.Scared : return scaredFace;
            case EmotionState.Agressive : return agressiveFace;
            case EmotionState.Angry: return angryFace;
            case EmotionState.Sleep: return sleepFace;
            case EmotionState.Suspicious: return suspiciousFace;
            case EmotionState.Curious: return curiousFace;
            case EmotionState.Love: return loveFace;
            case EmotionState.Tired: return tiredFace;
            case EmotionState.Stunned: return stunnedFace;
            case EmotionState.Swallow: return swallowFace;
            case EmotionState.Envy: return envyFace;
            case EmotionState.Friendly: return friendlyFace;
            case EmotionState.EnvyPacific: return envyPacificFace;
            default : return null;
        }
    }
}
