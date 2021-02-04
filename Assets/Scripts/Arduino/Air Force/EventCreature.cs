using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCreature : MonoBehaviour
{
    [SerializeField] 
    private int CreatureBirthLED = 0;
    [SerializeField]
    private int DeadCreatureLED = 1;
    [SerializeField]
    private int MealCreatureLED = 2;

    [SerializeField]
    private SerialHandler serialHandler = null;

    private void Start()
    {
        CreatureFactory.Instance.CreatureBirth += CreatureBirth;
        CreatureFactory.Instance.DeadCreature += DeadCreature;
        CreatureFactory.Instance.MealCreature += MealCreature;
    }

    private void CreatureBirth()
    {
        Debug.Log("Birth");
        StartCoroutine(CreatureBirthLed());
    }

    private void DeadCreature()
    {
        Debug.Log("Dead");
        StartCoroutine(DeadCreatureLed());
    }

    private void MealCreature()
    {
        Debug.Log("Meal");
        StartCoroutine(MealCreatureLed());
    }
    
    private IEnumerator CreatureBirthLed()
    {
        serialHandler.SetLed(CreatureBirthLED,100);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(CreatureBirthLED,0);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(CreatureBirthLED,100);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(CreatureBirthLED,0);
        yield return null;
    }

    private IEnumerator DeadCreatureLed()
    {
        serialHandler.SetLed(DeadCreatureLED,100);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(DeadCreatureLED,0);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(DeadCreatureLED,100);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(DeadCreatureLED,0);
        yield return null;
    }

    private IEnumerator MealCreatureLed()
    {
        serialHandler.SetLed(MealCreatureLED,100);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(MealCreatureLED,0);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(MealCreatureLED,100);
        yield return new WaitForSeconds(0.2f);
        serialHandler.SetLed(MealCreatureLED,0);
        yield return null;
    }

    private void OnDestroy()
    {
        CreatureFactory.Instance.CreatureBirth += CreatureBirth;
        CreatureFactory.Instance.DeadCreature += DeadCreature;
        CreatureFactory.Instance.MealCreature += MealCreature;
    }
}
