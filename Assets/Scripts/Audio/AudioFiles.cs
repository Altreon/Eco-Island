using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioFiles", menuName = "Audio/AudioFiles", order = 0)]
public class AudioFiles : ScriptableObject
{
    [Header("Ambiance")]
    [SerializeField]
    private SoundLoopClip dayMusic = null;
    [SerializeField]
    private SoundLoopClip ambianceDay = null;
    [SerializeField]
    private SoundLoopClip ambianceNight = null;
    [SerializeField]
    private SoundLoopClip ambianceRain = null;
    [SerializeField]
    private SoundLoopClip ambianceSnow = null;

    [Header("Creature Action")]
    [SerializeField]
    private RandomSoundOneShotClip creatureAttack = null;
    [SerializeField]
    private SingleSoundOneShotClip creatureSpawn = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureEatCreature = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureEatFood = null;

    //TODO un jeu de son pour différentes espèces
    [Header("Creature Reaction")]
    [SerializeField]
    private RandomSoundOneShotClip creatureAlert = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureFear = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureSpotPrey = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureDamagedWithoutResistance = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureDamagedWithResistance = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureHappy = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureHungry = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureSuspicious = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureCurious = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureLove = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureTired = null;
    [SerializeField]
    private RandomSoundOneShotClip creatureDefense = null;

    [Header("Ecodex")]
    [SerializeField] private SingleSoundOneShotClip ecoDexOpen = null;
    [SerializeField] private SingleSoundOneShotClip ecoDexClose = null;
    [SerializeField] private SingleSoundOneShotClip ecoDexPage = null;

    [Header("Player")]
    [SerializeField]
    private RandomSoundOneShotClip playerWalk = null;
    [SerializeField]
    private RandomSoundOneShotClip playerRun = null;
    [SerializeField] 
    private SingleSoundOneShotClip flashlight = null;
    
    [Header("Photo")]
    [SerializeField] private SingleSoundOneShotClip photoModeActive = null;
    [SerializeField] private SingleSoundOneShotClip photoModeDeactive = null;
    [SerializeField] private SingleSoundOneShotClip takePhoto = null;
    [SerializeField] private SingleSoundOneShotClip changeFlash = null;
    [SerializeField] private SingleSoundOneShotClip flashSoundEffect = null;
    [SerializeField] private SingleSoundOneShotClip changeFlashUP = null;
    [SerializeField] private SingleSoundOneShotClip changeFlashDOWN = null;
    [SerializeField] private SingleSoundOneShotClip changePhotoSound = null;
    [SerializeField] private SingleSoundOneShotClip deletePhotoSound = null;

    [Header("Other")]
    [SerializeField] private SingleSoundOneShotClip discoverCreature = null;
    
    public SoundOneShotClip SoundOneShotToClip (SoundOneShot sound) {
        switch (sound) {
            case SoundOneShot.CreatureAttack : return creatureAttack;
            case SoundOneShot.CreatureSpawn : return creatureSpawn;
            case SoundOneShot.CreatureEatCreature : return creatureEatCreature;
            case SoundOneShot.CreatureEatFood : return creatureEatFood;

            case SoundOneShot.CreatureAlert : return creatureAlert;
            case SoundOneShot.CreatureFear : return creatureFear;
            case SoundOneShot.CreatureSpotPrey : return creatureSpotPrey;
            case SoundOneShot.CreatureDamagedWithoutResistance : return creatureDamagedWithoutResistance;
            case SoundOneShot.CreatureDamagedWithResistance : return creatureDamagedWithResistance;
            case SoundOneShot.CreatureHappy : return creatureHappy;
            case SoundOneShot.CreatureHungry : return creatureHungry;
            case SoundOneShot.CreatureSuspicious : return creatureSuspicious;
            case SoundOneShot.CreatureCurious : return creatureCurious;
            case SoundOneShot.CreatureLove : return creatureLove;
            case SoundOneShot.CreatureTired: return creatureTired;
            case SoundOneShot.CreatureDefense: return creatureDefense;
            
            case SoundOneShot.EcodexOpen: return ecoDexOpen;
            case SoundOneShot.EcodexClose: return ecoDexClose;
            case SoundOneShot.EcodexPage: return ecoDexPage;
                
            case SoundOneShot.PhotomodeActive: return photoModeActive;
            case SoundOneShot.PhotomodeDeactive: return photoModeDeactive;
            case SoundOneShot.TakePhoto: return takePhoto;
            case SoundOneShot.FlashUI: return changeFlash;
            case SoundOneShot.FlashUI_DOWN: return changeFlashDOWN;
            case SoundOneShot.FlashUI_UP: return changeFlashUP;
            case SoundOneShot.FlashSoundEffect: return flashSoundEffect;
            
            case SoundOneShot.ChangePhotoSound : return changePhotoSound;
            case SoundOneShot.DeletePhotoSound : return deletePhotoSound;
            default : 
                Debug.LogError("SoundOneShotClip : " + sound + " was not found!");
                return null;

            case SoundOneShot.PlayerWalk : return playerWalk;
            case SoundOneShot.PlayerRun : return playerRun;
            case SoundOneShot.Flashlight : return flashlight;

            case SoundOneShot.DiscoverCreature : return discoverCreature;
        }
    }

    public SoundLoopClip SoundLoopToClip (SoundLoop sound) {
        switch (sound) {
            case SoundLoop.DayMusic : return dayMusic;
            case SoundLoop.AmbianceDay : return ambianceDay;
            case SoundLoop.AmbianceNight : return ambianceNight;
            case SoundLoop.AmbianceRain : return ambianceRain;
            case SoundLoop.AmbianceSnow : return ambianceSnow;
            default : 
                Debug.LogError("SoundLoopClip : " + sound + " was not found!");
                return null;
        }
    }
}

public enum SoundOneShot {
    CreatureAttack,
    CreatureSpawn,
    CreatureEatCreature,
    CreatureEatFood,

    CreatureAlert,
    CreatureFear,
    CreatureSpotPrey,
    CreatureDamagedWithoutResistance,
    CreatureDamagedWithResistance,
    CreatureHappy,
    CreatureHungry,
    CreatureSuspicious,
    CreatureCurious,
    CreatureLove,
    CreatureTired,
    CreatureDefense,
    
    EcodexOpen,
    EcodexClose,
    EcodexPage,
    
    PhotomodeActive,
    PhotomodeDeactive,
    TakePhoto,

    PlayerWalk,
    PlayerRun,
    Flashlight,
    
    FlashUI,
    FlashUI_UP,
    FlashUI_DOWN,
    FlashSoundEffect,
    
    ChangePhotoSound,
    DeletePhotoSound,

    DiscoverCreature
}

public enum SoundLoop {
    DayMusic,
    AmbianceDay,
    AmbianceNight,
    AmbianceRain,
    AmbianceSnow
}
