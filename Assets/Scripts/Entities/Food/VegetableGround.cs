using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableGround : SourceFood
{
    protected override void Awake () {
        FoodType = FoodType.Vegetable;
        base.Awake();
    }

    protected override Food CreateNewFood() {
        Vector3 randPointInZone = Random.insideUnitCircle * transform.lossyScale.x;
        Vector3 spawnPoint = transform.position + new Vector3(randPointInZone.x, 100, randPointInZone.y);
        Quaternion spawnRotation = Quaternion.identity;

        spawnPoint.y = 100;

        RaycastHit hit;
        if (Physics.Raycast(spawnPoint, Vector3.down, out hit, 200, LayerMask.GetMask("Terrain"))) {
            spawnPoint = hit.point + hit.normal * 0.01f;
            spawnRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);
        }else{
            spawnPoint = transform.position + transform.up * 0.01f;
        }

        Food food = FoodFactory.Instance.CreateFood(FoodType.Vegetable, spawnPoint, spawnRotation).GetComponent<Food>();
        food.transform.localScale = Vector3.zero;

        return food;
    }
}
