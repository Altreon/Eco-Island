using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticularityType {
    Explosion,
    Venom,
    Ghost,
    Spark,
    Rouli,
    Vacuum
}

public class ParticularitiesManager : MonoBehaviour
{
    public static ParticularitiesManager Instance {get; private set;}

    [SerializeField]
    private ExplosionConfig explosionConfig = null;
    [SerializeField]
    private VenomConfig venomConfig = null;
    [SerializeField]
    private GhostConfig ghostConfig = null;
    [SerializeField]
    private SparkConfig sparkConfig = null;
    [SerializeField]
    private RouliConfig rouliConfig = null;
    [SerializeField]
    private VacuumConfig vacuumConfig = null;

    public Particularity CreateParticularity (Creature owner, ParticularityType type) {
        switch (type) {
            case ParticularityType.Explosion : return new Explosion(owner, explosionConfig);
            case ParticularityType.Venom : return new Venom(owner, venomConfig);
            case ParticularityType.Ghost : return new Ghost(owner, ghostConfig);
            case ParticularityType.Spark : return new Spark(owner, sparkConfig);
            case ParticularityType.Rouli : return new Rouli(owner, rouliConfig);
            case ParticularityType.Vacuum : return new Vacuum(owner, vacuumConfig);
        }

        return null;
    } 

    private void Awake () {
        if(Instance){
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void OnDestroy () {
        if(Instance == this) Instance = null;
    }
}
