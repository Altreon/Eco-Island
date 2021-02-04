using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : ColorSwap
Système qui permet à la créature de changer de couleur
*/

public class ColorSwap
{
    private Color color;
    private float transparency = 1;

    private Renderer bodyRend;
    private Renderer hearLeftRend;
    private Renderer hearRightRend;
    private Renderer tailRend;
    private Renderer shadowRend;

    public ColorSwap (Renderer bodyRend, Renderer hearLeftRend, Renderer hearRightRend, Renderer tailRend, Renderer shadowRend) {
        this.bodyRend = bodyRend;
        this.hearLeftRend = hearLeftRend;
        this.hearRightRend = hearRightRend;
        this.tailRend = tailRend;
        this.shadowRend = shadowRend;

        Swap(bodyRend.material.GetColor("_MainColor"));
    }

    public void Swap (Color color){
        this.color = new Color(color.r, color.g, color.b, transparency);
        Color newColor = color;
        newColor.a = transparency;

        bodyRend.material.SetColor("_MainColor", color);

        hearLeftRend.material.SetColor("_Color", newColor);
        hearRightRend.material.SetColor("_Color", newColor);
        tailRend.material.SetColor("_Color", newColor);
    }

    public void SetTransparency (float transparency){
        this.transparency = transparency;
        Color newColor = color;
        newColor.a = transparency;

        bodyRend.material.SetFloat("_Transparent", transparency);

        hearLeftRend.material.SetColor("_Color", newColor);
        hearRightRend.material.SetColor("_Color", newColor);
        tailRend.material.SetColor("_Color", newColor);

        shadowRend.material.SetColor("_Color", new Color(0, 0, 0, transparency));
    }

    public Color GetColor () {
        return color;
    }

    public float GetTransparency () {
        return transparency;
    }
}
