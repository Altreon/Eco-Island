using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
Classe : Emotion
Système qui se charge de renvoyé la bonne émotion correspondant à l'état d'une créature
*/

public enum EmotionState { Default, Happy, Hungry, Dying, Scared, Agressive, Angry, Sleep, Suspicious, Curious, Love, Tired, Stunned, Swallow, Envy, Friendly, EnvyPacific};
public class Emotion
{
    private static Emotion _instance;
    public static Emotion Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Emotion();
            }
            return _instance;
        }
    }

    

    //@ensures Retourne l'émotion de la créature en fonction de l'état des buts de l'agent
    //private EmotionState GetEmotion(Agent agent)
    public EmotionState GetEmotion(Agent agent)
    {
        if(agent.Creature.DNADistortion.HaveParticularity(typeof(Vacuum))){
            if((agent.Creature.DNADistortion.GetParticularity(typeof(Vacuum)) as Vacuum).Actif){
                return EmotionState.Swallow;
            }
        }

        if (!agent.IsThinking) return EmotionState.Stunned;
        if (agent.Creature.isDying) return EmotionState.Dying;


        EmotionState result;
        Goal currentGoal =  agent.IsThinking? agent.Thinking.ActiveGoal : null;
        result = GoalToEmotion(currentGoal, agent.Creature);
        GoalComposite currentCompositeGoal = currentGoal as GoalComposite;

        while (currentCompositeGoal != null)
        {
            currentGoal = currentCompositeGoal.GetActiveGoal();
            if (currentGoal != null)
            {
                EmotionState whatEmotion = GoalToEmotion(currentGoal, agent.Creature);
                if (whatEmotion != EmotionState.Default)
                {
                    result = whatEmotion;
                    break;
                }
            }
            currentCompositeGoal = currentGoal as GoalComposite;
        }


        if (result == EmotionState.Default)
        {
            if (agent.Creature.Gauges.Hunger <= agent.Creature.Gauges.Hunger.MaxSize / 4) result = EmotionState.Hungry;
            if (agent.Memory.Meals.Read().Any() || agent.Memory.Communications.Read().Any(com => Time.time - com.RegistrationDate < 3)) result = EmotionState.Happy;


        }

        return result;
    }

    private EmotionState GoalToEmotion(Goal currentGoal, Creature creature)
    {
        if (currentGoal is GoalHunt) {
            GoalComposite cCG = currentGoal as GoalComposite;
            if (cCG != null)
            {
                if(cCG.GetActiveGoal() is GoalPursuit || cCG.GetActiveGoal() is GoalAttack
                   || cCG.GetActiveGoal() is GoalPursuitPlayer || cCG.GetActiveGoal() is GoalAttackPlayer)
                   return EmotionState.Agressive;
            }
        }

        if (currentGoal is GoalDefensePlayer) {
            GoalComposite cCG = currentGoal as GoalComposite;
            if (cCG != null)
            {
                if(cCG.GetActiveGoal() is GoalLookAtPlayer)
                   return EmotionState.Suspicious;
                if(cCG.GetActiveGoal() is GoalPursuitPlayer || cCG.GetActiveGoal() is GoalAttackPlayer)
                   return creature.Traits.Carnivorous.Value > 0.5 ? EmotionState.Agressive : EmotionState.Angry;
            }
        }

        if (currentGoal is GoalReproduction) {
            GoalComposite cCG = currentGoal as GoalComposite;
            if (cCG != null)
            {
                if(cCG.GetActiveGoal() is GoalReproduce)
                   return EmotionState.Love;
            }
        }

        if (currentGoal is GoalTired) {
            GoalComposite cCG = currentGoal as GoalComposite;
            if (cCG != null)
            {
                if(cCG.GetActiveGoal() is GoalSleep) {
                   return EmotionState.Sleep;
                }else{
                    return EmotionState.Tired;
                }
            }
        }
        
        if (currentGoal is GoalEvade || currentGoal is GoalEvadePlayer) return EmotionState.Scared;
        if (currentGoal is GoalDefense || currentGoal is GoalProtectPlayer) return EmotionState.Angry;
        if (currentGoal is GoalObservePlayer){
            if(Time.time > creature.agentCreature.Memory.Player.lastTimeFoodGive + 600){
                return EmotionState.Curious;
            }else{
                return EmotionState.Friendly;
            }
        }

        if(currentGoal is GoalEatInPlayerHand) {
            if(creature.Traits.Vigilance > 0.3){
                return EmotionState.Envy;
            }else{
                return EmotionState.EnvyPacific;
            }
        }
        
        return EmotionState.Default;
    }
}
