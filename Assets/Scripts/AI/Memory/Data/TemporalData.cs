using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TemporalData : Data
{
    public float ExpirationDate {get; private set;}

    protected TemporalData (float expirationTime) {
        ExpirationDate = Time.time + expirationTime;
    }
}
