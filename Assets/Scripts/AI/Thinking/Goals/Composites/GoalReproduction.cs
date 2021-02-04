using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalReproduction : GoalComposite
{
    private enum ReproductionState {
        Search,
        SeekNest,
        Wander,
        Reproduce
    }

    public GoalReproduction (Agent owner) : base(owner) {
        asked = new List<Creature>();
    }

    private ReproductionState reproductionState = ReproductionState.Search;
    private List<Creature> asked;

    public override void Activate () {
        base.Activate();

        AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

    
        IReadOnlyCollection<DataCreature> creatures = owner.Memory.Creatures.Read();
        Agent partenaire = null;
        float distanceToFriend = Mathf.Infinity;
        foreach(DataCreature data in creatures){
            if(!data.creature) continue;
            Agent agent = data.creature.agentCreature;
            if(asked.Contains(data.creature)) continue;

            if(data.RegistrationDate < Time.time - 0.5f || !agent.gameObject.activeSelf) continue;
            if(!agent.IsThinking || agent.IsThrow) continue;

            float distanceToAgent = Vector3.Distance(owner.transform.position, agent.transform.position);
            if(agent != owner && agent.Creature.SpecieID == owner.Creature.SpecieID && distanceToAgent < distanceToFriend){
                partenaire = agent;
                distanceToFriend = distanceToAgent;
            }
        }

        switch (reproductionState){
            case ReproductionState.Search :
                if(child.HasFailed) {
                    AddSubgoal(new GoalSeekNest(owner));
                    reproductionState = ReproductionState.SeekNest;
                }

                if(!partenaire) break;

                child.Abort();
                asked.Add(partenaire.Creature);

                if(partenaire.Thinking.RequestGoal(GoalReproduction_Evaluator.Instance)){
                    Squad squad = new Squad(owner, partenaire);
                    AddSubgoal(new GoalReproduce(owner, partenaire, squad));
                    partenaire.Thinking.ActiveGoal = new GoalReproduce(partenaire, owner, squad);
                    reproductionState = ReproductionState.Reproduce;
                }else{
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                }
                break;

            case ReproductionState.SeekNest :
                if(child.HasFailed) {
                    AddSubgoal(new GoalWander(owner));
                    reproductionState = ReproductionState.Wander;
                }

                if(!partenaire) break;

                child.Abort();
                asked.Add(partenaire.Creature);

                if(partenaire.Thinking.RequestGoal(GoalReproduction_Evaluator.Instance)){
                    Squad squad = new Squad(owner, partenaire);
                    AddSubgoal(new GoalReproduce(owner, partenaire, squad));
                    partenaire.Thinking.ActiveGoal = new GoalReproduce(partenaire, owner, squad);
                    reproductionState = ReproductionState.Reproduce;
                }else{
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                }
                break;

            case ReproductionState.Wander :
                if(!partenaire) break;

                child.Abort();
                asked.Add(partenaire.Creature);

                if(partenaire.Thinking.RequestGoal(GoalReproduction_Evaluator.Instance)){
                    Squad squad = new Squad(owner, partenaire);
                    AddSubgoal(new GoalReproduce(owner, partenaire, squad));
                    partenaire.Thinking.ActiveGoal = new GoalReproduce(partenaire, owner, squad);
                    reproductionState = ReproductionState.Reproduce;
                }else{
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                }
                break;

            case ReproductionState.Reproduce :
                if(child.HasFailed){
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                    reproductionState = ReproductionState.Search;
                }
                break;
        }

        base.ProcessSubgoals();
    }

    private Predicate<Creature> GetCreatureFilter () {
        return creature => {
            if(!creature.agentCreature.IsThinking) return false;
            if(creature.SpecieID != owner.Creature.SpecieID || asked.Contains(creature)) return false;
            return true;
        };
    }

    public override void Terminate () {
        base.Terminate();
    }
}
