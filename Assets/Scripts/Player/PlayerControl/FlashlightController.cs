using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
Classe : FlashLightController
Contrôle l'utilisation de la lampe torche par le joueur
*/

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioBox))]
public class FlashlightController : MonoBehaviour
{
    private Light Flashlight;

    [SerializeField]
    private SoundOneShot soundFlashlight = SoundOneShot.Flashlight;
    
    private bool FlashlightIsTurnOn = false;
    private AudioBox audioBox;

    private void Start()
    {
        Flashlight = GetComponent<Light>();
        audioBox = GetComponent<AudioBox>();
        Flashlight.enabled = false;
        InputManager.Input.PlayerGhost.Flashlight.performed += TurnOnAndTurnOffLight;
    }

    private void TurnOnAndTurnOffLight(InputAction.CallbackContext _context)
    {
        audioBox.PlayOneShot(soundFlashlight,1);

        if(!FlashlightIsTurnOn)
        {
            FlashlightIsTurnOn = true;
            Flashlight.enabled = true;
        }
        else
        {
            FlashlightIsTurnOn = false;
            Flashlight.enabled = false;
        }
    }

    public void ActiveForFlash(bool active){
        
        if(active){
            Flashlight.enabled = true;
            Flashlight.intensity = 3f;
            Flashlight.spotAngle = 60;
        }else{
            if(!FlashlightIsTurnOn) Flashlight.enabled = false;
            Flashlight.intensity = 2;
            Flashlight.spotAngle = 40;
        }
    }

    void OnDestroy () 
    {
        InputManager.Input.PlayerGhost.Flashlight.performed -= TurnOnAndTurnOffLight;
    }
}
