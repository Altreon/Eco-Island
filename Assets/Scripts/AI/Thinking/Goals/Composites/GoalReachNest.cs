using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalReachNest : GoalComposite
{
    public GoalReachNest (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();

        DataNest dataNest = owner.Memory.Nests.Read().FirstOrDefault(data => data.nest.SpecieID == owner.Creature.SpecieID);
        if(dataNest == null) {
            status = GoalStatus.Failed;
            return;
        }

        Vector3 nestPos = dataNest.nest.transform.position;
        Vector2 randPos = Random.insideUnitCircle;

        AddSubgoal(new GoalSeek(owner, nestPos + new Vector3(randPos.x, 0, randPos.y) * 10));
    }

    public override void Process () {
        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }
}
