using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSourceFood : TemporalData
{
    //TODO fichier config
    static float expirationTime = 600f;

    public SourceFood sourceFood;

    public DataSourceFood (SourceFood sourceFood) : base(expirationTime) {
        this.sourceFood = sourceFood;
    }

    public DataSourceFood (DataSourceFood dataSourceFood) : base(expirationTime - (Time.time - dataSourceFood.RegistrationDate)) {
        this.sourceFood = dataSourceFood.sourceFood;
    }
}
