using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Squad
Représente un groupe d'agents réuni pour accomplir un But commun
*/

public class Squad
{
    public Agent Leader {get; private set;}
    public List<Agent> Crew {get; private set;}
    public Goal_Evaluator Evaluator{get; private set;}

    public Squad (Agent leader, params Agent[] crew) {
        Leader = leader;
        Leader.Creature.CreatureDie += CreatureDied;

        Crew = new List<Agent>();
        foreach(Agent agent in crew){
            Add(agent);
        }
    }

    private void CreatureDied (Creature creature) {
        Remove(creature.agentCreature);
    }

    public void Add (Agent agent){
        Crew.Add(agent);
        agent.Creature.CreatureDie += CreatureDied;
    }

    public void Remove (Agent agent) {
        if(agent == Leader){
            if(Crew.Count >= 2){
                Leader = Crew[0];
                Crew.RemoveAt(0);
            }else{
                Leader = null;
            }
        }else{
            Crew.Remove(agent);
            if(Crew.Count == 0){
                Leader = null;
            }
        }

        agent.Creature.CreatureDie -= CreatureDied;
    }

    public bool IsDisband {
        get {
            return Leader == null;
        }
    }

    /*public void DebugLine () {
        Debug.DrawLine(Leader.transform.position, Crew[0].transform.position, Color.red);
    }*/
}
