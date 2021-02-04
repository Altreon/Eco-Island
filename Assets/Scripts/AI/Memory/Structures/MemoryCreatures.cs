using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCreatures : TemporalMemoryStruct
{
    List<DataCreature> creatures;

    public MemoryCreatures () {
        creatures = new List<DataCreature>();
    }

    public override void Write (Data data) {
        if(!(data is DataCreature)) return;
        DataCreature dataCreature = data as DataCreature;

        bool exist = false;
        int index = 0;
        foreach(DataCreature d in creatures){
            if(d.creature == dataCreature.creature){
                exist = true;
                break;
            }
            ++index;
        }

        if(exist){
            if(dataCreature.RegistrationDate >= creatures[index].RegistrationDate){
                Replace(new DataCreature(dataCreature), index);
            }
        }else{
            creatures.Add(new DataCreature(dataCreature));
        }
    }

    protected override void Remove (Data data) {
        if(!(data is DataCreature)) return;

        creatures.Remove(data as DataCreature);
    }

    public void RemoveByKey (Creature key) {
        DataCreature element = creatures.Find(data => data.creature == key);

        if(element != null) creatures.Remove(element); 
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataCreature)) return;

        creatures[index] = data as DataCreature;
    }

    protected override IEnumerable<Data> ReadData () {
        return creatures.AsReadOnly();
    }

    public IReadOnlyCollection<DataCreature> Read () {
        return creatures.AsReadOnly();
    }

    protected override int Count () {
        return creatures.Count;
    }
}

