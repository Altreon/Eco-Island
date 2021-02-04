using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCommunication : TemporalMemoryStruct
{
    List<DataCommunication> communications;

    public MemoryCommunication () {
        communications = new List<DataCommunication>();
    }

    public override void Write (Data data) {
        if(!(data is DataCommunication)) return;
        DataCommunication dataCommunication = data as DataCommunication;
        
        communications.Add(new DataCommunication(dataCommunication));
    }

    protected override void Remove (Data data) {
        if(!(data is DataCommunication)) return;

        communications.Remove(data as DataCommunication);
    }

    public void RemoveByKey (Creature key) {
        DataCommunication element = communications.Find(data => data.creature == key);

        if(element != null) communications.Remove(element); 
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataCommunication)) return;

        communications[index] = data as DataCommunication;
    }

    protected override IEnumerable<Data> ReadData () {
        return communications.AsReadOnly();
    }

    public IReadOnlyCollection<DataCommunication> Read () {
        return communications.AsReadOnly();
    }

    protected override int Count () {
        return communications.Count;
    }
}
