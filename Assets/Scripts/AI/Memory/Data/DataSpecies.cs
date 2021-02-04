using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataSpecies : Data
{
    //TODO config file
    public static int NB_MEAL = 3;

    public int SpeciesID {get; private set;}

    private List<CarnivorousFood> carnivorousFoods;
    public IReadOnlyCollection<CarnivorousFood> CarnivorousFoods {get{return carnivorousFoods;}}
    private List<HerbivorFood> herbivorFoods;
    public IReadOnlyCollection<HerbivorFood> HerbivorFoods {get{return herbivorFoods;}}
    public bool IsCarnivorous {get; private set;}

    public DataSpecies (int speciesID)  {
        SpeciesID = speciesID;
        carnivorousFoods = new List<CarnivorousFood>();
        herbivorFoods = new List<HerbivorFood>();
        IsCarnivorous = false;
    }

    public DataSpecies (DataSpecies dataSpecies)  {
        SpeciesID = dataSpecies.SpeciesID;
        carnivorousFoods = new List<CarnivorousFood>(dataSpecies.carnivorousFoods);
        herbivorFoods = new List<HerbivorFood>(dataSpecies.herbivorFoods);
        CheckIfCarnivorous();
    }

    public void addCarnivorousFood (CarnivorousFood food) {
        CarnivorousFood currentFood = carnivorousFoods.Find(f => f.preyID == food.preyID);
        if(currentFood != null){
            if(food.sawDate > currentFood.sawDate){
                carnivorousFoods[carnivorousFoods.IndexOf(currentFood)] = food;
                CheckIfCarnivorous();
            }
            return;
        }else if(carnivorousFoods.Count == NB_MEAL){
            CarnivorousFood oldestFood = carnivorousFoods[0];
            for(int i = 1; i < NB_MEAL; ++i){
                CarnivorousFood f = carnivorousFoods[i];
                if(f.sawDate < oldestFood.sawDate) oldestFood = f;
            }

            if(food.sawDate > oldestFood.sawDate){
                carnivorousFoods.Remove(oldestFood);
                carnivorousFoods.Add(food);
                CheckIfCarnivorous();
            }
        }else{
            carnivorousFoods.Add(food);
            CheckIfCarnivorous();
        }
    }

    public void removeCarnivorousFood (int preyID) {
        CarnivorousFood food = carnivorousFoods.Find(f => f.preyID == preyID);
        if(food == null) return;

        carnivorousFoods.Remove(food);
    }

    public void addHerbivorFood (HerbivorFood food) {
        HerbivorFood currentFood = herbivorFoods.Find(f => f.foodType == food.foodType);
        if(currentFood != null){
            if(food.sawDate > currentFood.sawDate){
                herbivorFoods[herbivorFoods.IndexOf(currentFood)] = food;
                CheckIfCarnivorous();
            }
            return;
        }else if(herbivorFoods.Count == NB_MEAL){
            HerbivorFood oldestFood = herbivorFoods[0];
            for(int i = 1; i < NB_MEAL; ++i){
                HerbivorFood f = herbivorFoods[i];
                if(f.sawDate < oldestFood.sawDate) oldestFood = f;
            }

            if(food.sawDate > oldestFood.sawDate){
                herbivorFoods.Remove(oldestFood);
                herbivorFoods.Add(food);
                CheckIfCarnivorous();
            }
        }else{
            herbivorFoods.Add(food);
            CheckIfCarnivorous();
        }
    }

    public void removeHerbivorFood (FoodType foodType) {
        HerbivorFood food = herbivorFoods.Find(f => f.foodType == foodType);
        if(food == null) return;

        herbivorFoods.Remove(food);
    }

    private void CheckIfCarnivorous () {
        float carnivorousEatLastDate = carnivorousFoods.Count != 0 ? carnivorousFoods.Max(food => food.sawDate) : -1;
        float herbivorEatLastDate = herbivorFoods.Count != 0 ? HerbivorFoods.Max(food => food.sawDate) : -1;

        IsCarnivorous = carnivorousEatLastDate > herbivorEatLastDate;
    }

    public IReadOnlyCollection<int> PreyIDs {
        get{
            return carnivorousFoods.Select(food => food.preyID).ToList().AsReadOnly();
        }
    }

    public IReadOnlyCollection<FoodType> EatFoodType {
        get{
            return herbivorFoods.Select(food => food.foodType).ToList().AsReadOnly();
        }
    }
}

public class CarnivorousFood {
    public int preyID {get; private set;}
    public float sawDate {get; private set;}

    public CarnivorousFood (int preyID, float sawDate) {
        this.preyID = preyID;
        this.sawDate = sawDate;
    }

    public CarnivorousFood (CarnivorousFood carnivorousFood) {
        this.preyID = carnivorousFood.preyID;
        this.sawDate = carnivorousFood.sawDate;
    }
}

public class HerbivorFood {
    public FoodType foodType {get; private set;}
    public float sawDate {get; private set;}

    public HerbivorFood (FoodType foodType, float sawDate) {
        this.foodType = foodType;
        this.sawDate = sawDate;
    }

    public HerbivorFood (HerbivorFood herbivorFood) {
        this.foodType = herbivorFood.foodType;
        this.sawDate = herbivorFood.sawDate;
    }
}
