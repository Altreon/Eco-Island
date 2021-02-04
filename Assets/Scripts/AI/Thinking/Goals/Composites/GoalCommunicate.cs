using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalCommunicate : GoalComposite
{
    private enum CommunicateState {
        Search,
        SeekNest,
        Wander,
        Share
    }

    private MemoryType memoryType;

    private List<Creature> asked;

    public GoalCommunicate (Agent owner, MemoryType memoryType) : base(owner) {
        this.memoryType = memoryType;

        asked = new List<Creature>();
    }

    private CommunicateState communicateState = CommunicateState.Search;

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
        Agent friend = null;
        float distanceToFriend = Mathf.Infinity;
        foreach(DataCreature data in creatures){
            if(!data.creature) continue;
            Agent agent = data.creature.agentCreature;
            if(agent == owner) continue;

            if(data.RegistrationDate < Time.time - 0.5f || !agent.gameObject.activeSelf) continue;

            if(!agent.IsThinking || agent.IsThrow) continue;

            float distanceToAgent = Vector3.Distance(owner.transform.position, agent.transform.position);
            if(distanceToAgent >= distanceToFriend) continue;

            /*bool recentTalk = false;
            IReadOnlyCollection<DataCommunication> communications = owner.Memory.Communications.Read();
            foreach(DataCommunication com in communications){
                if(com.creature == agent.Creature && com.subject == memoryType){
                    recentTalk = true;
                    break;
                }
            }*/

            if(GetCreatureFilter()(agent.Creature)){
                friend = agent;
                distanceToFriend = distanceToAgent;
            }
        }

        switch (communicateState){
            case CommunicateState.Search :
                if(child.HasFailed) {
                    AddSubgoal(new GoalSeekNest(owner));
                    communicateState = CommunicateState.SeekNest;
                }

                if(!friend) break;

                child.Abort();
                asked.Add(friend.Creature);

                if(friend.Thinking.RequestGoal(GoalCommunication_Evaluator.Instance)){
                    Squad squad = new Squad(owner, friend);
                    AddSubgoal(new GoalShare(owner, friend, memoryType, squad));
                    friend.Thinking.ActiveGoal = new GoalShare(friend, owner, memoryType, squad);
                    communicateState = CommunicateState.Share;
                }else{
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                }
                break;

            case CommunicateState.SeekNest :
                if(child.HasFailed) {
                    AddSubgoal(new GoalWander(owner));
                    communicateState = CommunicateState.Wander;
                }

                if(!friend) break;

                child.Abort();
                asked.Add(friend.Creature);

                if(friend.Thinking.RequestGoal(GoalCommunication_Evaluator.Instance)){
                    Squad squad = new Squad(owner, friend);
                    AddSubgoal(new GoalShare(owner, friend, memoryType, squad));
                    friend.Thinking.ActiveGoal = new GoalShare(friend, owner, memoryType, squad);
                    communicateState = CommunicateState.Share;
                }else{
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                }
                break;

            case CommunicateState.Wander :
                if(!friend) break;

                child.Abort();
                asked.Add(friend.Creature);

                if(friend.Thinking.RequestGoal(GoalCommunication_Evaluator.Instance)){
                    Squad squad = new Squad(owner, friend);
                    AddSubgoal(new GoalShare(owner, friend, memoryType, squad));
                    friend.Thinking.ActiveGoal = new GoalShare(friend, owner, memoryType, squad);
                    communicateState = CommunicateState.Share;
                }else{
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                }
                break;

            case CommunicateState.Share :
                if(child.HasFailed){
                    AddSubgoal(new GoalSearchCreature(owner, GetCreatureFilter()));
                    communicateState = CommunicateState.Search;
                }
                break;

        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        base.Terminate();
    }

    private Predicate<Creature> GetCreatureFilter () {
        return creature => {
            if(!creature.agentCreature.IsThinking) return false;
            if(creature.SpecieID != owner.Creature.SpecieID || asked.Contains(creature)) return false;
            IReadOnlyCollection<DataCommunication> communications = owner.Memory.Communications.Read();
            foreach(DataCommunication com in communications){
                if(com.creature == creature && com.subject == memoryType){
                    return false;
                }
            }
            return true;
        };
    }
}
