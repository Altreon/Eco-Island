using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFood : MonoBehaviour
{
    private void Start() {
        Food food = GetComponent<Food>();

        food.Eatable = true;
        food.Rigidbody.isKinematic = false;
        food.Rigidbody.AddForce(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);
        food.transform.localScale = Vector3.one;
        food.StartExpirationPeriod();

        Destroy(this);
    }
}
