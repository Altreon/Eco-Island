using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eating
{
    //TODO fichier config
    public static float timeToConsiderPlayerFood = 30;

    private float _eatDuration = 0.5f;
    private static Eating _instance;
    public static Eating Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new Eating();
            }
            return _instance;
        }
    }

    public void Eat(Creature predator, Creature prey, Action<bool> callback = null)
    {
        if (predator == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement Eat alors que la créature appelante est vide");
            return;
        }
        if (prey == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement Eat sur une cible Vide");
            return;
        }
        predator.StartCoroutine(EatBehaviour(predator, prey, callback));
    }

    private IEnumerator EatBehaviour (Creature predator, Creature prey, Action<bool> callback)
    {
        Transform predatorTransform = predator.transform;
        
        if(!predator.DNADistortion.HaveParticularity(typeof(Vacuum))
           || (predator.DNADistortion.GetParticularity(typeof(Vacuum)) as Vacuum).useTime < Time.time - 3){
            float startTime = Time.time;
            predator.UpdateSize = false;
            while (Time.time - startTime < _eatDuration)
            {
                //float taille = Mathf.Lerp(0.5f, 1, (Time.time - startTime)/_eatDuration);
                float taille = Mathf.Lerp(predator.SizeForAge, predator.SizeForAge + 0.5f, (Time.time - startTime)/_eatDuration);
                predatorTransform.localScale = Vector3.one * taille;
                yield return null;
            }
        }

        if (prey != null)
        {
            predator.Gauges.Hunger.Value = Mathf.Min(predator.Gauges.Hunger.MaxSize, predator.Gauges.Hunger + prey.nutritionnal);
            if (predator.agentCreature != null)
            {
                //predator.agentCreature.Memory.Meals.Write(new DataFood(FoodType.Meat, prey.transform.position, prey.gameObject));
                //TODO meal food
                predator.agentCreature.Memory.Meals.Write(new DataFood(null, prey.transform.position));
            }

            //Ajout de son
            predator.AudioBox.PlayOneShot(SoundOneShot.CreatureEatCreature);

            prey.Die();
            
            callback?.Invoke(true);
        }
        else
        {
            callback?.Invoke(false);
            Debug.LogWarning("Attention, la créature a été détruite pendant l'action de manger");
        }

        if(!predator.DNADistortion.HaveParticularity(typeof(Vacuum))
           || (predator.DNADistortion.GetParticularity(typeof(Vacuum)) as Vacuum).useTime < Time.time - 3){
            float startTime = Time.time;
            while (Time.time - startTime < _eatDuration)
            {
                //float taille = Mathf.Lerp(1, 0.5f, (Time.time - startTime)/_eatDuration);
                float taille = Mathf.Lerp(predator.SizeForAge + 0.5f, predator.SizeForAge, (Time.time - startTime)/_eatDuration);
                predatorTransform.localScale = Vector3.one * taille;
                yield return null;
            }

            predator.UpdateSize = true;
            predatorTransform.localScale = Vector3.one * predator.SizeForAge;
        }
    }










    public void EatFood(Creature predator, Food food, Action<bool> callback = null)
    {
        if (predator == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement Eat alors que la créature appelante est vide");
            return;
        }
        if (food == null)
        {
            Debug.LogWarning("Attention, vous appelez le comportement EatFood sur une cible Vide");
            return;
        }
        predator.StartCoroutine(EatFoodBehaviour(predator, food, callback));
    }

    private IEnumerator EatFoodBehaviour(Creature predator, Food food, Action<bool> callback)
    {
        if (food != null)
        {
            predator.Gauges.Hunger.Value = Mathf.Min(predator.Gauges.Hunger.MaxSize, predator.Gauges.Hunger + food.NutritiveValue);
            if (predator.agentCreature != null)
            {
                predator.agentCreature.Memory.Meals.Write(new DataFood(food, food.transform.position));
            }

            //Ajout de son
            predator.AudioBox.PlayOneShot(SoundOneShot.CreatureEatFood);
            
            float t = Time.time - timeToConsiderPlayerFood;
            if(food.LastPlayerHoldTime > t && predator.agentCreature.Memory.Player.lastSeeTime > t){
                Debug.Log("Player give a food to specie " + predator.SpecieID + " !");
                predator.AudioBox.PlayOneShot(SoundOneShot.CreatureHappy);
                predator.agentCreature.Memory.Player.lastTimeFoodGive = Time.time;
                Revision.Instance.ReviseCreatureEatPlayerFood(predator.agentCreature, predator, food.FoodType);
            }

            food.Delete();

            callback?.Invoke(true);
        }
        else
        {
            callback?.Invoke(false);
            Debug.LogWarning("Attention, la créature a été détruite pendant l'action de manger");
        }
        yield return null;
    }
}
