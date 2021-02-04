using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryNests : TemporalMemoryStruct
{
    List<DataNest> nests;

    public MemoryNests () {
        nests = new List<DataNest>();
    }

    public override void Write (Data data) {
        if(!(data is DataNest)) return;
        DataNest dataNest = data as DataNest;

        bool exist = false;
        int index = 0;
        foreach(DataNest d in nests){
            if(d.nest == dataNest.nest){
                exist = true;
                break;
            }
            ++index;
        }

        if(exist){
            if(dataNest.RegistrationDate >= nests[index].RegistrationDate){
                Replace(new DataNest(dataNest), index);
            }
        }else{
            nests.Add(new DataNest(dataNest));
        }
    }

    protected override void Remove (Data data) {
        if(!(data is DataNest)) return;

        nests.Remove(data as DataNest);
    }

    public void RemoveByKey (Nest key) {
        DataNest element = nests.Find(data => data.nest == key);

        if(element != null) nests.Remove(element); 
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataNest)) return;

        nests[index] = data as DataNest;
    }

    protected override IEnumerable<Data> ReadData () {
        return nests.AsReadOnly();
    }

    public IReadOnlyCollection<DataNest> Read () {
        return nests.AsReadOnly();
    }

    protected override int Count () {
        return nests.Count;
    }
}
