using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollision : MonoBehaviour
{
    private Creature myCreature;
    // Start is called before the first frame update
    void Start()
    {
        myCreature = GetComponent<Creature>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        myCreature.targetCreature = other.GetComponent<Creature>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (myCreature.targetCreature == other.GetComponent<Creature>())
        {
            myCreature.targetCreature = null;
        }
    }
}
