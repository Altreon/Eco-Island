using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFoodSources : TemporalMemoryStruct
{
    List<DataSourceFood> sourceFoods;

    public MemoryFoodSources () {
        sourceFoods = new List<DataSourceFood>();
    }

    public override void Write (Data data) {
        if(!(data is DataSourceFood)) return;
        DataSourceFood dataSourceFood = data as DataSourceFood;

        bool exist = false;
        int index = 0;
        foreach(DataSourceFood d in sourceFoods){
            if(d.sourceFood == dataSourceFood.sourceFood){
                exist = true;
                break;
            }
            ++index;
        }

        if(exist){
            if(dataSourceFood.RegistrationDate >= sourceFoods[index].RegistrationDate){
                Replace(new DataSourceFood(dataSourceFood), index);
            }
        }else{
            sourceFoods.Add(new DataSourceFood(dataSourceFood));
        }
    }

    protected override void Remove (Data data) {
        if(!(data is DataSourceFood)) return;

        sourceFoods.Remove(data as DataSourceFood);
    }

    public void RemoveByKey (SourceFood key) {
        DataSourceFood element = sourceFoods.Find(data => data.sourceFood == key);

        if(element != null) sourceFoods.Remove(element); 
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataSourceFood)) return;

        sourceFoods[index] = data as DataSourceFood;
    }

    protected override IEnumerable<Data> ReadData () {
        return sourceFoods.AsReadOnly();
    }

    public IReadOnlyCollection<DataSourceFood> Read () {
        return sourceFoods.AsReadOnly();
    }

    protected override int Count () {
        return sourceFoods.Count;
    }
}
