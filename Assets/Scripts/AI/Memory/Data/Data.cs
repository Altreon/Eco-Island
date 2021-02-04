using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Data
représente une information dans le monde du jeu
*/

public abstract class Data
{
    public float RegistrationDate {get; private set;}

    protected Data () {
        RegistrationDate = Time.time;
    }
}
