using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
Classe : FoodFactory
Factory prenant en charge la création de Food dans le monde du jeu
*/

public class FoodFactory : MonoBehaviour
{
    public static FoodFactory Instance {get; private set;}

    public bool activePooling = false;
    [SerializeField]
    private FoodPool fruitPool = null;
    [SerializeField]
    private FoodPool vegetablePool = null;

    private Stack<GameObject> fruitPooled;
    private Stack<GameObject> vegetablePooled;

    private GameObject fruitHolder;
    private GameObject vegetableHolder;

    private void Awake () {
        if(Instance){
            Destroy(this);
            return;
        }

        Instance = this;

        fruitPooled = new Stack<GameObject>();
        vegetablePooled = new Stack<GameObject>();
    }

    private void OnDestroy () {
        if(Instance == this) Instance = null;
    }

    private void Start () {
        fruitHolder = new GameObject("Fruit Holder");
        vegetableHolder = new GameObject("Vegetable Holder");

        if(!activePooling) return;

        for (int i = 0; i < fruitPool.amountToPool; ++i) {
            GameObject obj = Instantiate(fruitPool.prefab, Vector3.zero, Quaternion.identity, fruitHolder.transform);
            obj.SetActive(false); 
            fruitPooled.Push(obj);
        }

        for (int i = 0; i < vegetablePool.amountToPool; ++i) {
            GameObject obj = Instantiate(vegetablePool.prefab, Vector3.zero, Quaternion.identity, vegetableHolder.transform);
            obj.SetActive(false); 
            vegetablePooled.Push(obj);
        }
    }

    public GameObject CreateFood (FoodType type, Vector3 position, Quaternion rotation) {
        GameObject obj = null;
        switch (type) {

            case FoodType.Fruit :
                if(!activePooling){
                    obj = Instantiate(fruitPool.prefab, position, rotation, fruitHolder.transform);
                }else if(fruitPooled.Count != 0){
                    obj = fruitPooled.Pop();
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                }else{
                    obj = Instantiate(fruitPool.prefab, position, rotation, fruitHolder.transform);
                }
                break;
            
            case FoodType.Vegetable :
                if(!activePooling){
                    obj = Instantiate(vegetablePool.prefab, position, rotation, vegetableHolder.transform);
                }else if(vegetablePooled.Count != 0){
                    obj = vegetablePooled.Pop();
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                }else{
                    obj = Instantiate(vegetablePool.prefab, position, rotation, vegetableHolder.transform);
                }
                break;
            
            default :
                throw new ArgumentException("FoodType : " + type + " cannot be created with this food factory");
        }

        obj.SetActive(true);
        return obj;
    }

    public void DestroyFood (GameObject obj) {
        if(!activePooling){
            Destroy(obj);
            return;
        }

        if(obj.tag == "Fruit" && obj.gameObject.activeSelf){
            fruitPooled.Push(obj);
            obj.transform.parent = fruitHolder.transform;
        }else if(obj.tag == "Vegetable" && obj.gameObject.activeSelf){
            vegetablePooled.Push(obj);
            obj.transform.parent = vegetableHolder.transform;
        }else{
            throw new ArgumentException("Food : " + obj + " has not be created by this food factory");
        }

        Food food = obj.GetComponent<Food>();
        food.Eatable = false;
        food.StopAllCoroutines();
        food.Rigidbody.isKinematic = true;
        obj.SetActive(false);
        obj.transform.localScale = Vector3.one;
    }
}

[System.Serializable]
public class FoodPool {
    public GameObject prefab;
    [Min(0)]
    public int amountToPool = 0;
}
