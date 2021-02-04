using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : GoalSync
Dérivé de GoalComposite, qui ont la posibilité de se synchroniser avec d'autre agents d'une même Squad
*/
public abstract class GoalSync : GoalComposite
{
    public GoalSync (Agent owner) : base(owner) {}

    public abstract Goal_Evaluator Evaluator {get;}
}
