using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNest : TemporalData
{
    //TODO fichier config
    static float expirationTime = 6000f;

    public Nest nest;

    public DataNest (Nest nest) : base(expirationTime) {
        this.nest = nest;
    }

    public DataNest (DataNest dataNest) : base(expirationTime - (Time.time - dataNest.RegistrationDate)) {
        this.nest = dataNest.nest;
    }
}
