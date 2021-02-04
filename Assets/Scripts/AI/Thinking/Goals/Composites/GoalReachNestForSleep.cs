using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalReachNestForSleep : GoalComposite
{
    public GoalReachNestForSleep (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();

        DataNest dataNest = owner.Memory.Nests.Read().FirstOrDefault(data => data.nest.SpecieID == owner.Creature.SpecieID);
        if(dataNest == null) {
            status = GoalStatus.Failed;
            return;
        }

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        float distWithNest = dataNest != null ? Vector3.Distance(owner.transform.position, dataNest.nest.transform.position) : 0;
        float wantDistWithNest = desirabilitiesConfig.SlepConsiderationMinNestDistance + (desirabilitiesConfig.SlepConsiderationMaxNestDistance - desirabilitiesConfig.SlepConsiderationMinNestDistance)
                                 * desirabilitiesConfig.SleepToNestDesirabilityBySociability.Evaluate(owner.Creature.Traits.Sociability.Value);

        wantDistWithNest = Random.Range(desirabilitiesConfig.SlepConsiderationMinNestDistance, wantDistWithNest);
        Vector2 randPos = Random.insideUnitCircle;

        AddSubgoal(new GoalSeek(owner, dataNest.nest.transform.position + new Vector3(randPos.x, 0, randPos.y) * wantDistWithNest));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        if(Vector3.Distance(owner.transform.position, owner.Steering.Destination) < 1f){
            status = GoalStatus.Completed;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }
}
