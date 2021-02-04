using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
Classe : Agent
Classe représentant l'agent de jeu, et gère ses différentes modules d'intelligence artificielle
*/

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour
{
    public bool Debug = false;
    public bool IsThinking = true;
    public bool IsInit {get; private set;}
    public Steering Steering {get; private set;}
    public Perception Perception {get; private set;}
    public Thinking Thinking {get; private set;}
    public Memory Memory {get; private set;}
    public SoundEmission SoundEmission {get; private set;}

    public PerceptionConfig PerceptionConfig;

    //TODO Creature
    public Creature Creature {get; private set;}

    private Rigidbody rb;
    private NavMeshAgent nav;

    public bool IsThrow {get; private set;} = false;

    private void Awake () 
    {
        Creature = GetComponent<Creature>();
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start () {
        if(!IsInit) Init();

        StartCoroutine(UpdateEnumerator());
    }

    public void Init () {
        if(IsThinking) Perception = new Perception(this);
        Memory = new Memory(this);
        if(IsThinking) Thinking = new Thinking(this);
        Steering = new Steering(this);
        SoundEmission = new SoundEmission(this);

        InitPhysicsTraits();

        rb.velocity = Vector3.zero;
        nav.velocity = Vector3.zero;
        nav.ResetPath();

        IsInit = true;
    }

    private void InitPhysicsTraits () {
        nav.speed = Mathf.Lerp(2, 6, Creature.Traits.Speed.Value);
        Steering.MaxSpeed = nav.speed;
        if(IsThinking) Perception.PerceptionView.viewRadius = Mathf.Lerp(PerceptionConfig.viewRadius / 5, PerceptionConfig.viewRadius, Creature.Traits.Vision.Value);
        Creature.nutritionnal = (int) (Creature.Traits.Constitution.Value * 200) + 150;
    }

    private void Update () 
    {
        /*if(IsThinking) Perception.Update();
        Memory.Update();
        if(IsThinking && !IsThrow) Thinking.Update();
        if(!IsThrow) Steering.Update();
        SoundEmission.Update();*/
        //yield return new WaitForSeconds(0.05f);
    }

    private void FixedUpdate () {
        if(!IsThrow) Steering.UpdateMovement();
    }

    private void OnCollisionEnter(Collision collision){
        if(!IsThrow) return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Terrain")){
            nav.enabled = true;
            rb.isKinematic = true;

            IsThrow = false;
        }
    }

    private IEnumerator UpdateEnumerator () {
        while(enabled){
            if(IsThinking) Perception.Update();
            Memory.Update();
            if(IsThinking && !IsThrow) Thinking.Update();
            if(!IsThrow) Steering.Update();
            SoundEmission.Update();
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Throw (Vector3 dir, float force) {
        nav.enabled = false;
        rb.isKinematic = false;

        IsThrow = true;

        rb.AddForce(dir * force, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.value, Random.value, Random.value).normalized * force, ForceMode.Impulse);
    }
} 