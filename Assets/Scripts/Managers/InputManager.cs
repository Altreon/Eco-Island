using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
Classe : InputManager
Contrôle l'accès à la configuration des entrées clavier/souris/manette de l'application
*/

public class InputManager 
{
    private static Controls input;
    public static Controls Input 
    {
        get
        {
            if(input == null)
            {
                input = new Controls();
                input.Enable();

                InputSystem.onDeviceChange += DeviceChanged;
            }

            return input;
        }
    }

    public static void DeviceChanged (InputDevice device, InputDeviceChange change) {
        Debug.Log("ui");
    }
}
