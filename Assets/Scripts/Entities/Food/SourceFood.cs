using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : SourceFood
Représente une source de nourriture dans le monde du jeu, créant des Foods à intervales régulier
*/

public abstract class SourceFood : MonoBehaviour
{
    [Range(0, 300)]
    public float minFoodApparitionFrequence = 10;
    [Range(0, 300)]
    public float maxFoodApparitionFrequence = 100;
    [Range(0, 300)]
    public float minFoodMaturationDuration = 10;
    [Range(0, 300)]
    public float maxFoodMaturationDuration = 100;

    private List<FoodInMaturation> foodsInMaturation;

    public FoodType FoodType {get; protected set;}

    protected virtual void Awake () {
        foodsInMaturation = new List<FoodInMaturation>();
    }

    private void Start () {
        StartCoroutine(WaitForFoodApparition());
    }

    public void Update () {
        UpdateFoodMaturation();
    }

    private void UpdateFoodMaturation () {
        List<FoodInMaturation> foodsEndMaturation = new List<FoodInMaturation>();

        foreach(FoodInMaturation foodInMaturation in foodsInMaturation){
            float timeSinceAppaition = Time.time - foodInMaturation.apparitonTime;

            if(timeSinceAppaition >= foodInMaturation.maturationDuration){
                foodsEndMaturation.Add(foodInMaturation);
            }else{
                Food food = foodInMaturation.food;

                food.transform.localScale = Vector3.one * timeSinceAppaition / foodInMaturation.maturationDuration;
                if(food.FoodType == FoodType.Vegetable){
                    //food.transform.localPosition = new Vector3(food.transform.localPosition.x, timeSinceAppaition / foodInMaturation.maturationDuration);
                }
            }
        }

        foreach(FoodInMaturation foodInMaturation in foodsEndMaturation){
            foodsInMaturation.Remove(foodInMaturation);
            Food food = foodInMaturation.food;

            if(!food.gameObject.activeSelf){
                Debug.LogWarning("Food " + food.gameObject + "has been destroyed before maturation");
                continue;
            }

            food.Eatable = true;
            food.Rigidbody.isKinematic = false;
            food.Rigidbody.AddForce(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);
            food.transform.localScale = Vector3.one;
            food.StartExpirationPeriod();
        }
    }

    private IEnumerator WaitForFoodApparition () {
        while(enabled){
            yield return new WaitForSeconds(Random.Range(minFoodApparitionFrequence, maxFoodApparitionFrequence));

            Food food = CreateNewFood();

            foodsInMaturation.Add(new FoodInMaturation(food, Time.time, Random.Range(minFoodMaturationDuration, maxFoodMaturationDuration)));
        }
    }

    protected abstract Food CreateNewFood ();
}

class FoodInMaturation {
    public Food food;
    public float apparitonTime;
    public float maturationDuration;

    public FoodInMaturation (Food food, float apparitonTime, float maturationDuration){
        this.food = food;
        this.apparitonTime = apparitonTime;
        this.maturationDuration = maturationDuration;
    }
}
