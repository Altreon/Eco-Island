using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : MemoryStruct
Classe abstraite définissant un une classe gestionnaire de Datas pour l'agent
*/

public abstract class MemoryStruct
{
    public virtual void Update () {
        
    }

    public abstract void Write (Data data);

    protected abstract void Remove (Data data);

    protected abstract void Replace (Data data, int index);

    public void MergeFrom (MemoryStruct other, float percent = 1){
        if(percent > 1) {
            Debug.LogError("Merge percent cannot be > 1, percent = " + percent);
        }
        else if(percent == 1) {
            foreach(Data data in other.ReadData()) {
                Write(data);
            }
        }
        else if(percent > 0) {
            int n = Mathf.RoundToInt(other.Count() * percent);
            IEnumerator<Data> datas = other.ReadData().GetEnumerator();
            for (int i = 0; i < n; ++i){
                Write(datas.Current);
                datas.MoveNext();
            }
            datas.Dispose();
        }
        else if(percent < 0) {
            Debug.LogError("Merge percent cannot be < 1, percent = " + percent);
        }
    }

    protected abstract IEnumerable<Data> ReadData ();

    protected abstract int Count ();
}
