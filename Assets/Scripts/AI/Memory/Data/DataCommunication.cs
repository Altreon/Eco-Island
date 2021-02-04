using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCommunication  : TemporalData
{
    //TODO fichier config
    static float expirationTime = 60f;

    public Creature creature;
    public MemoryType subject;
    //TODO ajouter plus d'informatio sur la creature vue

    public DataCommunication (Creature creature, MemoryType subject) : base(expirationTime) {
        this.creature = creature;
        this.subject = subject;
    }

    public DataCommunication (DataCommunication dataCommunication) : base(expirationTime - (Time.time - dataCommunication.RegistrationDate)) {
        this.creature = dataCommunication.creature;
        this.subject = dataCommunication.subject;
    }
}
