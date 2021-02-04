using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSwap
{
    private FacesTextures faces;
    private Renderer rend;

    public FaceSwap (Renderer rend) {
        faces = GameManager.Instance.FacesTextures;
        this.rend = rend;
    }

    public void Swap (EmotionState emotion){
        rend.material.SetTexture("_FaceTex", faces.GetFace(emotion));
    }
    
}
