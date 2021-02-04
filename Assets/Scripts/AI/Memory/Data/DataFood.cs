using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFood : TemporalData
{
    //TODO fichier config
    static float expirationTime = 10f;

    public Food food;
    public Vector3 lastPos;

    public DataFood (Food food, Vector3 lastPos) : base(expirationTime) {
        this.food = food;
        this.lastPos = lastPos;
    }

    public DataFood (DataFood dataFood) : base(expirationTime - (Time.time - dataFood.RegistrationDate)) {
        this.food = dataFood.food;
        this.lastPos = dataFood.lastPos;
    }
}
