using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rouli : Particularity
{
    private RouliConfig config;

    public Rouli (Creature owner, RouliConfig config) : base(owner) {
        this.config = config;
    }

    public override bool PrepareCondition () {
        return false;
    }

    public override void Prepare () {
       
    }

    public void Appear () {
        
    }

    public override bool ActivationCondition () {
        return false;
    }

    public override void Activation () {
        
    }

    public override void Inactif () {
        
    }

    public override void Destroy () {
        
    }
}
