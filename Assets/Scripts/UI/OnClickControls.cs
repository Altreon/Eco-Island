using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickControls : MonoBehaviour
{
    [SerializeField] private Sprite touchpad_controls = null;
    [SerializeField] private Sprite mouse_controls = null;
    [SerializeField] private Sprite gamepad_controls = null;
    [SerializeField] private UnityEngine.UI.Image controlImage = null;

    public void OnClickTouchpad()
    {
        controlImage.sprite = touchpad_controls;
        controlImage.color = Color.white;
    }
    
    public void OnClickMouse()
    {
        controlImage.sprite = mouse_controls;
        controlImage.color = Color.white;
    }
    
    public void OnClickGamepad()
    {
        controlImage.sprite = gamepad_controls;
        controlImage.color = Color.white;
    }

    public void Back()
    {
        controlImage.sprite = null;
        controlImage.color = Color.clear;
    }
}
