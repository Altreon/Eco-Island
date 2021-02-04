using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Food
Classe représentant un objet nourriture dans le monde du jeu
*/

public enum FoodType {
    Fruit,
    Vegetable,
    Meat
}

public class Food : MonoBehaviour
{
    [SerializeField]
    private FoodType foodType = FoodType.Fruit;
    public FoodType FoodType{get{return foodType;}}

    [SerializeField]
    private float expirationTime = 30;

    [SerializeField]
    [Min(0)]
    private int nutritiveValue = 0;
    public int NutritiveValue{get{return nutritiveValue;}}

    [SerializeField]
    private float heightToDestroy = 0;


    public Rigidbody Rigidbody{get; private set;}

    public bool Eatable {get; set;} = false;

    public float LastPlayerHoldTime {get; set;} = 0;

    private void Awake () {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update () {
        if(transform.position.y < heightToDestroy){
            StopAllCoroutines();
            Delete();
        }
    }

    public void StartExpirationPeriod () {
        StartCoroutine(WaitAndDestroyAfterExpirationTime());
    }

    private IEnumerator WaitAndDestroyAfterExpirationTime () {
        yield return new WaitForSeconds(expirationTime);

        Delete();
    }

    public void Delete () {
        ParticuleManager.Instance.CreateParticle(ParticleType.FoodDesapear, transform.position, Quaternion.identity);
        FoodFactory.Instance.DestroyFood(gameObject);
    }
}
