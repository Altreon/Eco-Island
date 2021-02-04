using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMeals : TemporalMemoryStruct
{
    List<DataFood> meals;

    public MemoryMeals () {
        meals = new List<DataFood>();
    }

    public override void Write (Data data) {
        if(!(data is DataFood)) return;

        meals.Add(new DataFood(data as DataFood));
    }

    protected override void Remove (Data data) {
        if(!(data is DataFood)) return;

        meals.Remove(data as DataFood);
    }

    public void RemoveByKey (Food key) {
        DataFood element = meals.Find(data => data.food == key);

        if(element != null) meals.Remove(element); 
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataFood)) return;

        meals[index] = data as DataFood;
    }

    protected override IEnumerable<Data> ReadData () {
        return meals.AsReadOnly();
    }

    public IReadOnlyCollection<DataFood> Read () {
        return meals.AsReadOnly();
    }

    protected override int Count () {
        return meals.Count;
    }
}
