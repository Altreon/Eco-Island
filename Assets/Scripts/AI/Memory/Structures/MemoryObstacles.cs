using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryObstacles : TemporalMemoryStruct
{
    List<DataObstacle> obstacles;

    public MemoryObstacles () {
        obstacles = new List<DataObstacle>();
    }

    public override void Write (Data data) {
        if(!(data is DataObstacle)) return;
        DataObstacle dataObstacle = data as DataObstacle;

        bool exist = false;
        int index = 0;
        foreach(DataObstacle d in obstacles){
            if(d.collider == dataObstacle.collider){
                exist = true;
                break;
            }
            ++index;
        }

        if(exist){
            if(dataObstacle.RegistrationDate >= obstacles[index].RegistrationDate){
                Replace(new DataObstacle(dataObstacle), index);
            }
        }else{
            obstacles.Add(new DataObstacle(dataObstacle));
        }
    }

    protected override void Remove (Data data) {
        if(!(data is DataObstacle)) return;

        obstacles.Remove(data as DataObstacle);
    }

    public void RemoveByKey (Collider key) {
        DataObstacle element = obstacles.Find(data => data.collider == key);

        if(element != null) obstacles.Remove(element); 
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataObstacle)) return;

        obstacles[index] = data as DataObstacle;
    }

    protected override IEnumerable<Data> ReadData () {
        return obstacles.AsReadOnly();
    }

    public IReadOnlyCollection<DataObstacle> Read () {
        return obstacles.AsReadOnly();
    }

    protected override int Count () {
        return obstacles.Count;
    }
}
