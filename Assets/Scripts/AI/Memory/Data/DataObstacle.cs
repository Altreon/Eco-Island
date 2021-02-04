using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObstacle : TemporalData
{
    //TODO fichier config
    static float expirationTime = 120f;

    public Collider collider;

    public DataObstacle (Collider collider) : base(expirationTime) {
        this.collider = collider;
    }

    public DataObstacle (DataObstacle dataObstacle) : base(expirationTime - (Time.time - dataObstacle.RegistrationDate)) {
        this.collider = dataObstacle.collider;
    }
}
