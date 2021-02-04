using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCommunication : GoalComposite
{
    public GoalCommunication (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();

        MemoryType memoryType = MemoryType.Species;
        List<SubjectPropability> subjectPropabilities = GameManager.Instance.DesirabilitiesConfig.CommunicationSubjetPropabilities;
        float p = Random.value;
        float f = 0;
        for(int i = 0; i < subjectPropabilities.Count; ++i){
            SubjectPropability sp = subjectPropabilities[i];
            f += sp.Probability;
            if(p <= f){
                memoryType = sp.Subject;
                break;
            }
        }

        AddSubgoal(new GoalCommunicate(owner, memoryType));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }
}
