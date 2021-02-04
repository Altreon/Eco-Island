using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDefensePlayer : GoalComposite
{
    //TODO config
    float minDistToAttack = 2;
    float maxDistToAttack = 15;
    float minDistToPursuit = 10;
    float maxDistToPursuit = 20;

    float timeStart = 0;

    private enum DefenseState {
        LookAt,
        Pursuit,
        Attack,
    }

    private DefenseState defenseState = DefenseState.LookAt;

    public GoalDefensePlayer (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();

        timeStart = Time.time;
    }

    public override void Process () {
        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        float vigilanceConsideration = desirabilitiesConfig.DefensePlayerConsiderationMinVigilance;

        float vigilance = (desirabilitiesConfig.DefensePlayerDesirabilityByAggressivity.Evaluate(owner.Creature.Traits.Vigilance.Value) - vigilanceConsideration) * (1 / (1 - vigilanceConsideration));
        vigilance *= (Player.Instance.PlayerController.Velocity.magnitude * desirabilitiesConfig.DefensePlayerPlayerSpeedThreatWeight);
        float distToAttack = Mathf.Lerp(minDistToAttack, maxDistToAttack, vigilance);
        float distToPursuit = Mathf.Lerp(minDistToPursuit, maxDistToPursuit, vigilance);

        Vector3 playerPos = Player.Instance.transform.position;
        float distanceToPlayer = Vector3.Distance(owner.transform.position, playerPos);

        Goal child = GetActiveGoal();
        if(child != null && child.IsInactive){
            child.Activate();
        }

        switch (defenseState){
            case DefenseState.LookAt :

                if(owner.Steering.Behavior != eSteeringBehavior.LookAtPlayer){
                    AddSubgoal(new GoalLookAtPlayer(owner));
                }
                else if(distanceToPlayer < 2){
                    if(child != null) child.Abort();
                    AddSubgoal(new GoalAttackPlayer(owner));
                    defenseState = DefenseState.Attack;
                    timeStart = Time.time;
                }else if(Time.time - timeStart >= desirabilitiesConfig.DefensePlayerMaxTimeSpend) {
                    status = GoalStatus.Completed;
                }else{
                    IReadOnlyCollection<DataCreature> friends = owner.Memory.Creatures.Read();
                    bool friendAggresed = false;
                    Agent friend = null;
                    foreach(DataCreature data in friends){
                        if(!data.creature) continue;
                        Agent agent = data.creature.agentCreature;

                        if(!agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

                        if(Vector3.Distance(agent.transform.position, playerPos) < distToAttack){
                            friendAggresed = true;
                            friend = agent;
                            break;
                        }
                    }

                    if(!friendAggresed) {
                        friendAggresed = Vector3.Distance(owner.transform.position, playerPos) < distToAttack;
                        friend = owner;
                    }

                    if(friendAggresed){
                        if(child != null) child.Abort();
                        AddSubgoal(new GoalPursuitPlayer(owner));
                        defenseState = DefenseState.Pursuit;
                        if (Random.value > 0.5f) owner.Creature.AudioBox.PlayOneShot(SoundOneShot.CreatureDefense);
                        timeStart = Time.time;
                    }
                }
                break;
            
            case DefenseState.Pursuit :
                if(child.IsComplete) {
                    AddSubgoal(new GoalAttackPlayer(owner));
                    defenseState = DefenseState.Attack;
                }
                else if(child.HasFailed) {
                    defenseState = DefenseState.LookAt;
                }else if (Vector3.Distance(owner.transform.position, playerPos) > distToPursuit) {
                    child.Abort();
                    defenseState = DefenseState.LookAt;
                }
                break;
            case DefenseState.Attack :
                if(child.IsComplete || child.HasFailed) {
                    IReadOnlyCollection<DataCreature> friends = owner.Memory.Creatures.Read();
                    bool friendAggresed = false;
                    Agent friend = null;
                    foreach(DataCreature data in friends){
                        if(!data.creature) continue;
                        Agent agent = data.creature.agentCreature;

                        if(!agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 1f) continue;

                        if(Vector3.Distance(agent.transform.position, playerPos) < distToAttack){
                            friendAggresed = true;
                            friend = agent;
                            break;
                        }
                    }

                    if(!friendAggresed) {
                        friendAggresed = Vector3.Distance(owner.transform.position, playerPos) < distToAttack;
                        friend = owner;
                    }

                    if(friendAggresed){
                        if(child != null) child.Abort();
                        AddSubgoal(new GoalPursuitPlayer(owner));
                        defenseState = DefenseState.Pursuit;
                    }else{
                        AddSubgoal(new GoalLookAtPlayer(owner));
                        defenseState = DefenseState.LookAt;
                    }
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        owner.Memory.Player.lastObserved = Time.time;
        base.Terminate();
    }
}
