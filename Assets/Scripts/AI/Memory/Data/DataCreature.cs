using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCreature : TemporalData
{
    //TODO fichier config
    static float expirationTime = 60f;

    public Creature creature;
    public Vector3 lastPos;
    //TODO ajouter plus d'informatio sur la creature vue

    public DataCreature (Creature creature, Vector3 lastPos) : base(expirationTime) {
        this.creature = creature;
        this.lastPos = lastPos;
    }

    public DataCreature (DataCreature dataCreature) : base(expirationTime - (Time.time - dataCreature.RegistrationDate)) {
        this.creature = dataCreature.creature;
        this.lastPos = dataCreature.lastPos;
    }
}
