using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFoodRevision : MonoBehaviour
{
    public List<Agent> agentsToRevise = null;
    public float reviseAfterTime = 60f;

    private void Update() {
        reviseAfterTime -= Time.deltaTime;

        if(reviseAfterTime <= 0){
            if(agentsToRevise.Count != 0) Revise();
            Destroy(this);
        } 
    }

    private void Revise () {
        foreach(Agent agentToRevise in agentsToRevise){
            Revision.Instance.ReviseCreatureEatFood(agentToRevise, agentToRevise.Creature, FoodType.Vegetable);
        }
        Debug.Log("REVISION DONE");
    }
}
