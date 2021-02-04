using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalEvade : GoalComposite
{
    private enum EvadeState {
        Flee,
        Hide
    }

    private EvadeState evadeState = EvadeState.Flee;

    public GoalEvade (Agent owner) : base(owner) {}

     public override void Activate () {
        base.Activate();

        AddSubgoal(new GoalFlee(owner));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        bool enemyClose = false;
        bool obstacleClose = false;

        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();

        foreach(DataCreature data in creatures){
            Agent agent = data.creature?.agentCreature;
            if(!agent || !agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 5f) continue;

            bool isHostil = CreatureIsHostil(owner, agent.Creature);
            if(isHostil && Vector3.Distance(owner.transform.position, agent.transform.position) < owner.PerceptionConfig.viewRadius / 4){
                enemyClose = true;
                break;
            }
        }

        DataObstacle obstacle = owner.Memory.Obstacles.Read().FirstOrDefault(data => MaxColliderSize(data.collider) >= 2f);
        obstacleClose = obstacle != null;

        switch (evadeState) {
            case EvadeState.Flee :
                if(!enemyClose && obstacleClose){
                    child.Abort();
                    AddSubgoal(new GoalHide(owner));
                    evadeState = EvadeState.Hide;
                }
                break;
            
            case EvadeState.Hide :
                if(enemyClose){
                    child.Abort();
                    AddSubgoal(new GoalFlee(owner));
                    evadeState = EvadeState.Flee;
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        owner.Steering.Behavior = eSteeringBehavior.Idle;

        base.Terminate();
    }

    private float MaxColliderSize(Collider collider) {
        Vector3 size = collider.bounds.size;

        /*if(size.x >= size.y && size.x >= size.z) return size.x;
        if(size.y >= size.x && size.y >= size.z) return size.y;
        return size.z;*/

        return size.x >= size.z ? size.x : size.y;
    }

    public static bool CreatureIsHostil (Agent owner, Creature other) {
        if(owner.Creature.SpecieID == other.SpecieID) return false;

        DataSpecies data = owner.Memory.Species.GetByKey(other.SpecieID);
        if(data == null) return false;
        if(owner.Debug) {
            /*Debug.Log("List : " + other.SpecieID);
            foreach(int i in data.preyIDs){
                Debug.Log("i");
            }*/
        }

        bool isEatMe = data.IsCarnivorous && data.PreyIDs.Contains(owner.Creature.SpecieID);
        if(other.agentCreature.IsThinking){
            if(!isEatMe) return false;
        }else{

            if(!isEatMe && (other.agentCreature.Steering.Target == null || other.agentCreature.Steering.Target.Creature.SpecieID != owner.Creature.SpecieID)) return false;
        }

        //if(other.Traits.Carnivorous.Value < 0.5f) return false;

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;
        return Vector3.Distance(owner.transform.position, other.transform.position) < desirabilitiesConfig.EvadeConsiderationMaxDistance / 2;
    }
}
