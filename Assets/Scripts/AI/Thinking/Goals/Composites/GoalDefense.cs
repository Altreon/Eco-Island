using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDefense : GoalComposite
{
    private enum DefenseState {
        LookAt,
        Pursuit,
        Attack,
    }

    private DefenseState defenseState = DefenseState.LookAt;

    public GoalDefense (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child != null && child.IsInactive){
            child.Activate();
        }

        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        Agent aggresor = null;
        float distanceToAggresor = Mathf.Infinity;
        foreach(DataCreature data in creatures){
            if(!data.creature) continue;
            Agent agent = data.creature.agentCreature;

            if(!agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

            bool isHostil = GoalEvade.CreatureIsHostil(owner, agent.Creature);
            if(!isHostil) continue;

            //bool attackSpecies = agent.Steering.Target && agent.Steering.Target.Creature.SpecieID == owner.Creature.SpecieID;
            //if(!attackSpecies) continue;

            float distanceToAgent = Vector3.Distance(owner.transform.position, agent.transform.position);

            if(agent != owner && agent.Creature.SpecieID != owner.Creature.SpecieID && distanceToAgent < distanceToAggresor){
                aggresor = agent;
                distanceToAggresor = distanceToAgent;
            }
        }

        switch (defenseState){
            case DefenseState.LookAt :
                if(aggresor == null){
                    if(child != null) child.Abort();
                }
                else if(owner.Steering.Behavior != eSteeringBehavior.LookAt){
                    AddSubgoal(new GoalLookAt(owner, aggresor));
                }
                /*else if(aggresor.Steering.Target != owner){
                    if(child != null) child.Abort();
                    AddSubgoal(new GoalPursuit(owner, aggresor));
                    defenseState = DefenseState.Pursuit;
                }*/
                else if(distanceToAggresor < 2){
                    if(child != null) child.Abort();
                    AddSubgoal(new GoalAttack(owner, aggresor.Creature));
                    defenseState = DefenseState.Attack;
                }else{
                    IReadOnlyCollection<DataCreature> friends = owner.Memory.Creatures.Read();
                    bool friendAggresed = false;
                    foreach(DataCreature data in friends){
                        if(!data.creature) continue;
                        Agent agent = data.creature.agentCreature;

                        if(!agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

                        if(aggresor.Steering.Target == agent){
                            friendAggresed = true;
                            break;
                        }
                    }

                    if(friendAggresed){
                        if(child != null) child.Abort();
                        AddSubgoal(new GoalPursuit(owner, aggresor));
                        if (Random.value > 0.5f) owner.Creature.AudioBox.PlayOneShot(SoundOneShot.CreatureDefense);
                        defenseState = DefenseState.Pursuit;
                    }
                }
                break;
            
            case DefenseState.Pursuit :
                if(child.IsComplete) {
                    AddSubgoal(new GoalAttack(owner, owner.Steering.Target.Creature));
                    defenseState = DefenseState.Attack;
                }
                else if(child.HasFailed) {
                    defenseState = DefenseState.LookAt;
                }
                else if(aggresor != null) {                    
                    if((owner.Steering.Behavior == eSteeringBehavior.Pursuit || owner.Steering.Behavior == eSteeringBehavior.Seek)
                        && aggresor != owner.Steering.Target && Vector3.Distance(owner.transform.position, aggresor.transform.position) + 5f < Vector3.Distance(owner.transform.position, owner.Steering.Destination)){
                        child.Abort();
                        AddSubgoal(new GoalPursuit(owner, aggresor));
                    }
                }
                break;
            case DefenseState.Attack :
                if(aggresor == null) break;
                if(child.IsComplete || child.HasFailed) {
                    IReadOnlyCollection<DataCreature> friends = owner.Memory.Creatures.Read();
                    bool friendAggresed = false;
                    foreach(DataCreature data in friends){
                        if(!data.creature) continue;
                        Agent agent = data.creature.agentCreature;

                        if(!agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

                        if(aggresor.Steering.Target == agent){
                            friendAggresed = true;
                            break;
                        }
                    }

                    if(friendAggresed){
                        if(child != null) child.Abort();
                        AddSubgoal(new GoalPursuit(owner, aggresor));
                        defenseState = DefenseState.Pursuit;
                    }else{
                        AddSubgoal(new GoalLookAt(owner, aggresor));
                        defenseState = DefenseState.LookAt;
                    }

                    /*if(aggresor.Steering.Target != owner){
                        AddSubgoal(new GoalPursuit(owner, aggresor));
                        defenseState = DefenseState.Pursuit;
                    }else{
                        AddSubgoal(new GoalLookAt(owner, aggresor));
                        defenseState = DefenseState.LookAt;
                    }*/
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }
}
