using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Particularity
{
    protected Creature owner;

    public Particularity (Creature owner) {
        this.owner = owner;
    }

    public abstract bool PrepareCondition () ;
    public abstract void Prepare () ;

    public abstract bool ActivationCondition () ;
    public abstract void Activation () ;

    public abstract void Inactif () ;

    public abstract void Destroy () ;
}
