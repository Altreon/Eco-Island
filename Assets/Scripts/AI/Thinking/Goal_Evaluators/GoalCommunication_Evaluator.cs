using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCommunication_Evaluator : Goal_Evaluator
{
    private static GoalCommunication_Evaluator instance;
    public static GoalCommunication_Evaluator Instance {
        get{
            if(instance == null){
                instance = new GoalCommunication_Evaluator();
            }

            return instance;
        }
    }

    public override float CalculateDesirability (Agent agent, bool requested) {
        if(!agent.Creature.MetabolismActive) return 0;

        DesirabilitiesConfig desirabilitiesConfig = GameManager.Instance.DesirabilitiesConfig;

        float lastComTime = desirabilitiesConfig.CommunicationConsiderationMaxTime;
        IReadOnlyCollection<DataCommunication> communications = agent.Memory.Communications.Read();
            foreach(DataCommunication com in communications){
                float time = Time.time - com.RegistrationDate;
                if(time < lastComTime) lastComTime = time;
            }

        float desirability = desirabilitiesConfig.CommunicationDesirabilityByTime.Evaluate(lastComTime / desirabilitiesConfig.CommunicationConsiderationMaxTime)
                            * desirabilitiesConfig.CommunicationDesirabilityBySociability.Evaluate(agent.Creature.Traits.Sociability.Value)
                            * desirabilitiesConfig.CommunicationDesirabilityTwicker;

        if(agent.Thinking.ActiveGoal?.GetType() == typeof(GoalCommunication) ||
           agent.Thinking.ActiveGoal?.GetType() == typeof(GoalShare)) desirability *= desirabilitiesConfig.CommunicationConfirmationBias;
        if(requested) desirability *= desirabilitiesConfig.CommunicationRequestBias;

        return desirability;
    }

    public override Goal CreateGoal (Agent agent) {
        return new GoalCommunication(agent);
    }

    public override System.Type GetGoalType () {
        return typeof(GoalCommunication);
    }
}

[System.Serializable]
public class SubjectPropability {
    [SerializeField]
    private MemoryType subject = MemoryType.Foods;
    public MemoryType Subject {get{return subject;}}

    [SerializeField]
    private float probability = 0f;
    public float Probability {get{return probability;}}
}