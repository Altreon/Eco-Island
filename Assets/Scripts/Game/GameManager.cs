using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : GameManager
Manager permettant d'accéder à plusieurs fichier de configuration de la simulation
*/

public class GameManager : MonoBehaviour
{
    static public GameManager Instance{get; private set;}

    [Range(0, 10)]
    public float TimeScale = 1f;

    [Header("Configurations")]
    [SerializeField]
    private DesirabilitiesConfig desirabilitiesConfig = null;
    [SerializeField]
    private RevisionConfig revisionConfig = null;
    [SerializeField]
    private FacesTextures facesTextures = null;

    private void Awake () {
        if(Instance){
            Destroy(this);
            return;
        }

        Instance = this;

        //AudioListener.pause = true;
    }

    private void OnDestroy () {
        if(Instance == this) Instance = null;
    }

    private void Update () {
        if(Time.timeScale != TimeScale) Time.timeScale = TimeScale;
    }

    public DesirabilitiesConfig DesirabilitiesConfig {get {return desirabilitiesConfig;}}
    public RevisionConfig RevisionConfig {get {return revisionConfig;}}
    public FacesTextures FacesTextures {get {return facesTextures;}}
}
