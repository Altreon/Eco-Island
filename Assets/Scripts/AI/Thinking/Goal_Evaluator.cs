using System.Collections;
using System.Collections.Generic;

/**
Classe : Goal_Evaluator
Permet d'évaluer la désirabilités que l'agent a pour un but, représenté par un nombre flotant
*/

public abstract class Goal_Evaluator
{
    public abstract float CalculateDesirability (Agent agent, bool requested);

    public abstract Goal CreateGoal (Agent agent);

    public abstract System.Type GetGoalType ();
}
