using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Perception
Module de percepion, se charge de récolter des informaitons perceptibles dans l'environnement
*/

public class Perception 
{
    public PerceptionView PerceptionView {get; private set;}
    public RevisionView RevisionView {get; private set;}
    public PerceptionSound PerceptionSound {get; private set;}

    public bool ViewActive {get; set;} = true;

    public Perception(Agent owner)
    {
        PerceptionView = new PerceptionView(owner);
        RevisionView = new RevisionView(owner);
        PerceptionSound = new PerceptionSound(owner);
    }

    public void Update() 
    {
        if(ViewActive) PerceptionView.Update();
    }
}
