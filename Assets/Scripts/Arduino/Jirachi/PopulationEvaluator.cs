using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationEvaluator : MonoBehaviour
{
    [SerializeField] 
    private int Led1specie = 0;
    [SerializeField]
    private int Led2specie = 1;
    [SerializeField]
    private int Led3specie = 2;

    [SerializeField]
    private SerialHandler serialHandler = null;
    private void Awake()
    {
        if (!serialHandler)
        {
            enabled = false;
        }
    }
    private void OnEnable()
    {
        //CreatureFactory.creaturePopulationChangedEvent += PopulationChangedToLed;
    }

    private void Start()
    {
        StartCoroutine(InitLeds());
    }

    private IEnumerator InitLeds()
    {
        while (!serialHandler.IsSerialReady)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        serialHandler.SetLed(1, creatureCountToPurcentage(CreatureFactory.Instance.CreatureCountTable[Led1specie]));
        serialHandler.SetLed(2, creatureCountToPurcentage(CreatureFactory.Instance.CreatureCountTable[Led2specie]));
        serialHandler.SetLed(3, creatureCountToPurcentage(CreatureFactory.Instance.CreatureCountTable[Led3specie]));
    }

    private void PopulationChangedToLed()
    {
        serialHandler.SetLed(1, creatureCountToPurcentage(CreatureFactory.Instance.CreatureCountTable[Led1specie]));
        serialHandler.SetLed(2, creatureCountToPurcentage(CreatureFactory.Instance.CreatureCountTable[Led2specie]));
        serialHandler.SetLed(3, creatureCountToPurcentage(CreatureFactory.Instance.CreatureCountTable[Led3specie]));
        Debug.Log("--------------CreatureList-------------");
        for (int specie = 0; specie < 3; specie++)
        {
            Debug.Log(CreatureFactory.Instance.CreatureCountTable[specie]);
        }
        Debug.Log("-----------");
    }

    private float creatureCountToPurcentage(float creatureNumber)
    {
        return Mathf.Min(creatureNumber / 15f, 1f);
    }

    private void OnDisable()
    {
        CreatureFactory.creaturePopulationChangedEvent -= PopulationChangedToLed;
    }
}
