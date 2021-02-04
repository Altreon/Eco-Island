using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : TemporalMemoryMemory
Dérivé de MemoryStruct, dont les Datas finissent par être détruites avec le temps
*/

public abstract class TemporalMemoryStruct : MemoryStruct
{
    public override void Update () {
        ExpirationData();
    }

    private void ExpirationData () {
        Stack<Data> dataToRemove = new Stack<Data>();

        foreach(TemporalData data in ReadData()) {
            if(Time.time >= data.ExpirationDate) dataToRemove.Push(data);
        }

        foreach(Data data in dataToRemove) {
            Remove(data);
        }

        dataToRemove.Clear();
    }
}
