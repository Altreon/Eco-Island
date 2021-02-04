using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEatInPlayerHand : GoalComposite
{

    private enum EatInPlayerHandState {
        Catch,
        Eat,
    }

    private EatInPlayerHandState eatInPlayerHandState = EatInPlayerHandState.Catch;

    public GoalEatInPlayerHand (Agent owner) : base(owner) {}

    public override void Activate () {
        base.Activate();

        if(!Player.Instance.PlayerPickAndDrop.IsHandleFood){
            status = GoalStatus.Failed;
            return;
        }

        owner.Steering.IsSlow = true;

        AddSubgoal(new GoalCatch(owner, Player.Instance.PlayerPickAndDrop.TransportableFruit.transform));
    }

    public override void Process () {
        Goal child = GetActiveGoal();
        if(child.IsInactive){
            child.Activate();
        }

        if(!Player.Instance.PlayerPickAndDrop.IsHandleFood){
            status = GoalStatus.Failed;
            return;
        }

        switch (eatInPlayerHandState){
            case EatInPlayerHandState.Catch :
                if(child.IsComplete) {
                    Player.Instance.PlayerPickAndDrop.DropOffFood();

                    AddSubgoal(new GoalEatFood(owner, owner.Steering.Aim));
                    eatInPlayerHandState = EatInPlayerHandState.Eat;

                    owner.Creature.currentEmotion = EmotionState.Friendly;
                }
                break;
        }

        base.ProcessSubgoals();
    }

    public override void Terminate () {
        owner.Steering.IsSlow = false;

        //owner.Memory.Player.lastObserved = Time.time;
        base.Terminate();
    }
}
