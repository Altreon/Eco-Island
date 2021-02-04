using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : Creature
Classe représentant une créature dans le monde du jeu, ainsi que l'ensemble de ses composants
*/

public class Creature : MonoBehaviour
{
    [SerializeField]
    public GameObject Body;

    public int SpecieID;
    //public int maxLifeNumber = 10;
    public bool isDying = false;
    public float Age = 0;
    public bool UpdateSize = true;
    public float GrowSpeed = 0.01f;
    //private int _lifeNumber;
    public Agent agentCreature { get; private set; }

    public EmotionState currentEmotion = EmotionState.Default;

    [Header("Renderers")]
    [SerializeField]
    private Renderer bodyRenderer = null;
    [SerializeField]
    private Renderer hearLeftRenderer = null;
    [SerializeField]
    private Renderer hearRightRenderer = null;
    [SerializeField]
    private Renderer tailRenderer = null;
    [SerializeField]
    private Renderer shadowRenderer = null;

    public Creature targetCreature { get; set; }
    public FaceSwap FaceSwap {get; private set;}
    public ColorSwap ColorSwap {get; private set;}
    public MignonBody BodyShader {get; private set;}

    public AudioBox AudioBox {get; private set;}

    public bool MetabolismActive {get; set;} = true;

    //EVENT
    public Action<Creature> CreatureDie;

    private void OnDisable()
    {
        //TODO réactiver après mise en place de la factory
        //_traits = null;
        //_gauges = null;
    }

    [SerializeField] private CreatureTraits _traits;
    public CreatureTraits Traits
    {
        get { return _traits; }
        set
        {
            _traits = value;
        }
    }
    
    [SerializeField] private CreatureGauges _gauges;
    private bool isGaugesSubscribed = false;
    public CreatureGauges Gauges
    {
        get { return _gauges; }
        set
        {
            /*if (gameObject.activeSelf == false && _gauges == null)
                _gauges = value;
            else
                Debug.LogWarning("Can't change gauges while creature active");*/

            /*if(_gauges != null){
                _gauges.Life.Depleted -= Die;
                _gauges.Hunger.Depleted -= Die;
            }*/

            isGaugesSubscribed = true;

            _gauges = value;
            /*_gauges.Life.Depleted += Die;
            _gauges.Hunger.Depleted += Die;*/
        }
    }

    /*public int LifeNumber {
        get
        {
            return _lifeNumber;
        }
        set
        {
            if (value <= 0) Die();
            else _lifeNumber = value;
        }
    }*/
    public int attackPower = 2;

    public int nutritionnal = 30;

    private float lastTime;

    public CreatureDoing CreatureDoing { get; private set; }

    public DNADistortion DNADistortion {get; private set;}

    private void Awake()
    {
        agentCreature = GetComponent<Agent>();
        //_gauges.InitializeGauges(_traits);
        //stomachValue = Gauges.Hunger;
        //_lifeNumber = maxLifeNumber;

        //reproductionValue = maxReproductionValue;

        CreatureDoing = new CreatureDoing(this);
        CreatureDoing.myCreature = this;

        DNADistortion = new DNADistortion();

        Renderer rend = Body.GetComponent<Renderer>();
        BodyShader = Body.GetComponent<MignonBody>();

        FaceSwap = new FaceSwap(rend);
        ColorSwap = new ColorSwap(bodyRenderer, hearLeftRenderer, hearRightRenderer, tailRenderer, shadowRenderer);

        AudioBox = GetComponent<AudioBox>();

        /*if(!isGaugesSubscribed){
            _gauges.Life.Depleted -= Die;
            _gauges.Hunger.Depleted -= Die;
        }*/
    }

    private void OnDestroy () {

    }
    
    void Start()
    {
        lastTime = Time.time;
    }

    void Update()
    {
        if(MetabolismActive){
            Age += GrowSpeed * Time.deltaTime;
            AudioBox.SetOneShotPitch(1 + 0.5f * Age - 0.25f);
        }

        if (agentCreature != null){
            EmotionState deductedEmotion = Emotion.Instance.GetEmotion(agentCreature);
            if(deductedEmotion != currentEmotion && Time.time - lastTime > 1f){
                currentEmotion = deductedEmotion;
                FaceSwap.Swap(deductedEmotion);
                lastTime = Time.time;

                switch(currentEmotion){
                    //case EmotionState.Happy : AudioBox.PlayOneShot(SoundOneShot.CreatureHappy); break;
                    case EmotionState.Hungry : AudioBox.PlayOneShot(SoundOneShot.CreatureHungry); break;
                    case EmotionState.Scared : PlayFearSound(); break;
                    case EmotionState.Suspicious: if (UnityEngine.Random.value > 0.5f) AudioBox.PlayOneShot(SoundOneShot.CreatureSuspicious); break;
                    case EmotionState.Agressive: AudioBox.PlayOneShot(SoundOneShot.CreatureSpotPrey); break;
                    case EmotionState.Curious: AudioBox.PlayOneShot(SoundOneShot.CreatureCurious); break;
                    case EmotionState.Love: AudioBox.PlayOneShot(SoundOneShot.CreatureLove); break;
                    case EmotionState.Tired: AudioBox.PlayOneShot(SoundOneShot.CreatureTired); break;
                }
            }
        }

        DNADistortion.Update();

        //TODO les delegué des gauges fonctionne pas
        if(MetabolismActive) _gauges.UpdateGauges(Time.deltaTime);
        if (_gauges.Life <= 0 || _gauges.Hunger <= 0 || Age > 1) Die();

        if(UpdateSize) transform.localScale = Vector3.one * SizeForAge;
    }

    public float SizeForAge {get{ return Age + 0.5f;}}

    public void Die(bool withEffect = true)
    {
        if(withEffect) PlayDieEffect();

        DNADistortion.DestroyAllParticularities();

        isDying = true;
        if(agentCreature.IsThinking) agentCreature.Perception.RevisionView.RemoveAllEvent();
        //
        CreatureDoing.StopCommunicate();
        //
        CreatureDie?.Invoke(this);
        //Destroy(gameObject);
        CreatureFactory.Instance?.DestroyCreature(this);
    }

    private void PlayDieEffect () {
        ParticleSystem particle = ParticuleManager.Instance.CreateParticle(ParticleType.Death, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particle.main;
        ParticleSystem.MainModule dropletsMain = particle.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        ParticleSystem.ColorOverLifetimeModule skullColorOverLifetime = particle.transform.GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime;
        Color color = ColorSwap.GetColor();

        main.startColor = color;
        dropletsMain.startColor = color;

        Gradient grad = new Gradient();
        grad.SetKeys( new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f), new GradientAlphaKey(1.0f, 0.9f), new GradientAlphaKey(0.0f, 0.0f) } );
        skullColorOverLifetime.color = grad;
    }

    private void PlayFearSound () {
        bool chased = false;

        IReadOnlyCollection<DataCreature> creatures = agentCreature.Memory.Creatures.Read();
        foreach(DataCreature data in creatures){
            Agent agent = data.creature?.agentCreature;
            if(!agent || !agent.gameObject.activeSelf || data.RegistrationDate < Time.time - 5f) continue;

            bool isHostil = GoalEvade.CreatureIsHostil(agentCreature, agent.Creature);
            if(isHostil && agent.Steering.Target == agentCreature){
                chased = true;
                break;
            }
        }

        //Ajout de son
        if(chased){
            AudioBox.PlayOneShot(SoundOneShot.CreatureFear);
        }else{
            if (UnityEngine.Random.value > 0.2f) AudioBox.PlayOneShot(SoundOneShot.CreatureAlert);
        }
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunBehaviour(duration));   
    }

    private IEnumerator StunBehaviour(float duration)
    {
        Revision.Instance.ReviseCreatureFlashedByPlayer(agentCreature, this);

        agentCreature.Thinking.ActiveGoal = null;
        agentCreature.IsThinking = false;
        
        currentEmotion = EmotionState.Stunned;
        FaceSwap.Swap(currentEmotion);

        GameObject stunnedParticle = ParticuleManager.Instance.CreateParticle(ParticleType.StunStars, transform.position + new Vector3(0, 0.5f, 0), transform.rotation * Quaternion.Euler(-90, 0, 0));
        stunnedParticle.transform.parent = transform;
        yield return new WaitForSeconds(duration);
        ParticuleManager.Instance.DestroyParticle(ParticleType.StunStars, stunnedParticle);
        yield return RecoverBehavior();
    }

    //TODO: A polish s'il y a le temps
    private IEnumerator RecoverBehavior()
    {
        yield return null;
        agentCreature.IsThinking = true;
    }
}
