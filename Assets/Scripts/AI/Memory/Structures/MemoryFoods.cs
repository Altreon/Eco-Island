using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFoods : TemporalMemoryStruct
{
    List<DataFood> foods;

    public MemoryFoods () {
        foods = new List<DataFood>();
    }

    public override void Write (Data data) {
        if(!(data is DataFood)) return;
        DataFood dataFood = data as DataFood;

        bool exist = false;
        int index = 0;
        foreach(DataFood d in foods){
            if(d.food == dataFood.food){
                exist = true;
                break;
            }
            ++index;
        }

        if(exist){
            if(dataFood.RegistrationDate >= foods[index].RegistrationDate){
                Replace(new DataFood(dataFood), index);
            }
        }else{
            foods.Add(new DataFood(dataFood));
        }
    }

    protected override void Remove (Data data) {
        if(!(data is DataFood)) return;

        foods.Remove(data as DataFood);
    }

    public void RemoveByKey (Food key) {
        DataFood element = foods.Find(data => data.food == key);

        if(element != null) foods.Remove(element); 
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataFood)) return;

        foods[index] = data as DataFood;
    }

    protected override IEnumerable<Data> ReadData () {
        return foods.AsReadOnly();
    }

    public IReadOnlyCollection<DataFood> Read () {
        return foods.AsReadOnly();
    }

    protected override int Count () {
        return foods.Count;
    }
}
