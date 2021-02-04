using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MemorySpecies : MemoryStruct
{
    List<DataSpecies> species;

    public MemorySpecies () {
        species = new List<DataSpecies>();
    }

    public override void Write (Data data) {
        if(!(data is DataSpecies)) return;
        DataSpecies dataSpecies = data as DataSpecies;

        DataSpecies sameData = species.Find(d => d.SpeciesID == dataSpecies.SpeciesID);

        if(sameData != null){
            /*sameData.preyIDs = sameData.preyIDs.Union(dataSpecies.preyIDs).ToList();
            sameData.eatFoodTypes = sameData.eatFoodTypes.Union(dataSpecies.eatFoodTypes).ToList();*/
            foreach(CarnivorousFood food in dataSpecies.CarnivorousFoods){
                sameData.addCarnivorousFood(new CarnivorousFood(food));
            }
            foreach(HerbivorFood food in dataSpecies.HerbivorFoods){
                sameData.addHerbivorFood(new HerbivorFood(food));
            }
        }else{
            species.Add(new DataSpecies(dataSpecies));
        }
    }

    protected override void Remove (Data data) {
        if(!(data is DataSpecies)) return;

        species.Remove(data as DataSpecies);
    }

    public void RemoveByKey (int key) {
        DataSpecies element = species.Find(data => data.SpeciesID == key);

        if(element != null) species.Remove(element);
    }

    public DataSpecies GetByKey (int key) {
        return species.Find(data => data.SpeciesID == key);
    }

    public bool ContainKey (int key) {
        DataSpecies element = species.Find(data => data.SpeciesID == key);

        return element != null;
    }

    protected override void Replace (Data data, int index) {
        if(!(data is DataSpecies)) return;

        species[index] = data as DataSpecies;
    }

    protected override IEnumerable<Data> ReadData () {
        return species.AsReadOnly();
    }

    public IReadOnlyCollection<DataSpecies> Read () {
        return species.AsReadOnly();
    }

    protected override int Count () {
        return species.Count;
    }
}
