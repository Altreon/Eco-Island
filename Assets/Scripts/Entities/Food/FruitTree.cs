using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : SourceFood
{
    [SerializeField]
    private Transform[] foodSpawnZones = null;

    protected override void Awake () {
        FoodType = FoodType.Fruit;
        base.Awake();
    }

    protected override Food CreateNewFood() {
        Transform spawnZone = foodSpawnZones[Random.Range(0, foodSpawnZones.Length)]; 
        Vector3 spawnPos = spawnZone.position + Random.insideUnitSphere * spawnZone.lossyScale.x;

        Food food = FoodFactory.Instance.CreateFood(FoodType.Fruit, spawnPos, Quaternion.identity).GetComponent<Food>();
        food.transform.localScale = Vector3.zero;

        return food;
    }
}
