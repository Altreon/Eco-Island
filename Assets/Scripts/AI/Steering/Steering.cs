using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

/**
Classe : Steering
Module de Locomotion, permet à l'agent de se déplacer dans le monde du jeu
*/

public enum eSteeringBehavior {
    Idle,
    Wander,
    Seek,
    Pursuit,
    PursuitPlayer,
    Catch,
    Flee,
    FleePlayer,
    Hide,
    HidePlayer,
    LookAt,
    LookAtPlayer
}

public class Steering
{
    //TODO fichier config
    private float offsetDist = 0.6f;
    //TODO fichier config
    //private float offsetVel = 0.5f;

    public Action<Agent, eSteeringBehavior> SteeringBehaviorChanged;

    private Agent owner;
    private SteeringBehavior behavior = Idle.Instance;

    private NavMeshAgent nav;
    private NavMeshPath computingPath;
    //private bool canComputePath = true;
    public Vector3 Velocity {get; private set;}
    public float MaxSpeed {get; set;}
    public bool IsSlow {get; set;}

    public Vector3 Destination {get; private set;}
    public Agent Target {get; set;}
    public Quaternion TargetRotation {get; set;}
    public Transform Aim {get; set;}
    public List<Agent> Evades {get; set;}

    private Transform body;

    private float rotationSpeed = 5;
    

    private float moveS = 0;
    private float moveSSpeed = 8;
    private float moveSPercent = 0.6f;

    private float rotationSSpeed = 1;
    private float rotationS2Speed = 2;
    private float rotationSXAngle = 60;
    private float rotationSYAngle = 60;

    private float rotateRouliSpeed = 2f;
    private float rotateRouliR = 270;

    public Steering (Agent owner) {
        this.owner = owner;

        nav = owner.GetComponent<NavMeshAgent>();
        computingPath = new NavMeshPath();

        Evades = new List<Agent>();

        body = owner.Creature.Body.transform;

        TargetRotation = owner.transform.rotation;
    }

    public void Update () {
        if(behavior == null) Behavior = eSteeringBehavior.Idle;

        if(computingPath.status == NavMeshPathStatus.PathComplete){
            if(!nav.isOnNavMesh){
                Debug.LogWarning("Creature Die Because off of navmesh");
                owner.Creature.Die();
                return;
            }
            nav.SetPath(computingPath);
            computingPath = new NavMeshPath();
        }

        Evades.RemoveAll(agent => agent == null);

        Velocity = nav.velocity;

        behavior.Update(owner);

        if(owner.Debug){
            Vector3[] path = nav.path.corners;
            for(int i = 0; i < path.Length - 1; ++i){
                Debug.DrawLine(path[i], path[i+1], Color.red);
            }
        }
    }

    public void UpdateMovement () {
        if(owner.Creature.DNADistortion.HaveParticularity(typeof(Rouli))){
            MoveByRouli();
            RotateByRouli();
        }else{
            MoveByS();
            RotateTo();
        }
    }

    private void MoveByS () {
        if(Behavior == eSteeringBehavior.Idle || Behavior == eSteeringBehavior.LookAt || Behavior == eSteeringBehavior.LookAtPlayer || nav.path == null || nav.path.corners.Length <= 1) return;

        moveS += (IsSlow? moveSSpeed / 2 : moveSSpeed) * (0.5f + owner.Creature.Traits.Speed.Value) * Time.deltaTime;

        float sin = Mathf.Sin(moveS);
        float cos = Mathf.Cos(moveS);
        nav.speed = MaxSpeed - Mathf.Abs(sin) * MaxSpeed * moveSPercent;
        if(IsSlow) nav.speed /= 2;

        Vector3 rotateDir = (nav.path.corners[1] - owner.transform.position).normalized;
        Quaternion bodyRot = Quaternion.Euler(-90, 0, 0);
        float angleCos = rotationSXAngle * cos;
        rotateDir = Quaternion.Euler(0, angleCos, 0) * rotateDir;
        bodyRot *= Quaternion.Euler(rotationSYAngle * Mathf.Sin(moveS * 2), 0, 0);

        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, 
            Quaternion.LookRotation(rotateDir, Vector3.up), rotationSSpeed * Time.deltaTime);
        body.localRotation = Quaternion.Slerp(body.localRotation, bodyRot, rotationS2Speed * Time.deltaTime);
    }

    private void RotateTo () {
        if(Behavior != eSteeringBehavior.LookAt && Behavior != eSteeringBehavior.LookAtPlayer) return;

        owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, TargetRotation, rotationSpeed * Time.deltaTime);

        if(Behavior == eSteeringBehavior.LookAtPlayer){
            Quaternion bodyRot = Quaternion.LookRotation((CameraController.Instance.transform.position + Vector3.down * 0.5f) - owner.transform.position) * Quaternion.Euler(-90, 0, 0);
            body.rotation = Quaternion.Slerp(body.rotation, bodyRot, rotationS2Speed * Time.deltaTime);
        }
    }

    private void MoveByRouli () {
        if(Behavior == eSteeringBehavior.Idle || Behavior == eSteeringBehavior.LookAt || Behavior == eSteeringBehavior.LookAtPlayer || nav.path == null || nav.path.corners.Length <= 1) return;

        moveS += (IsSlow? moveSSpeed / 2 : moveSSpeed) * (0.5f + owner.Creature.Traits.Speed.Value) * Time.deltaTime;

        float sin = Mathf.Sin(moveS);
        float cos = Mathf.Cos(moveS);

        nav.speed = MaxSpeed - Mathf.Abs(sin) * MaxSpeed * moveSPercent;
        if(IsSlow) nav.speed /= 2;

        rotateRouliR += rotateRouliSpeed * nav.speed;
        if(rotateRouliR > 180f) rotateRouliR = rotateRouliR - 360;
        Quaternion bodyRot = Quaternion.Euler(rotateRouliR, 0, 0);

        Vector3 rotateDir = (nav.path.corners[1] - owner.transform.position).normalized;
        float angleCos = rotationSXAngle * cos;
        rotateDir = Quaternion.Euler(0, angleCos, 0) * rotateDir;

        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, 
            Quaternion.LookRotation(rotateDir, Vector3.up), rotationSSpeed * Time.deltaTime);
        body.localRotation = Quaternion.Slerp(body.localRotation, bodyRot, 10 * Time.deltaTime);
    }

    private void RotateByRouli () {
        if(Behavior != eSteeringBehavior.LookAt && Behavior != eSteeringBehavior.LookAtPlayer) return;

        rotateRouliR = 270;
        owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, TargetRotation, rotationSpeed * Time.deltaTime);

        if(Behavior == eSteeringBehavior.LookAtPlayer){
            Quaternion bodyRot = Quaternion.LookRotation((CameraController.Instance.transform.position + Vector3.down * 0.5f) - owner.transform.position) * Quaternion.Euler(-90, 0, 0);
            body.rotation = Quaternion.Slerp(body.rotation, bodyRot, rotationS2Speed * Time.deltaTime);
        }else{
            body.localRotation = Quaternion.Slerp(body.localRotation, Quaternion.Euler(-90, 0, 0), 8 * Time.deltaTime);
        }
    }

    public eSteeringBehavior Behavior {
        get{return behavior.BehaviorType;}
        set{
            if(behavior != null){
                if(behavior.BehaviorType == value) return;
                behavior.Exit(owner);
            }

            switch (value) {
                case eSteeringBehavior.Idle :
                    behavior = Idle.Instance;
                    break;
                case eSteeringBehavior.Wander :
                    behavior = Wander.Instance;
                    break;
                case eSteeringBehavior.Seek :
                    behavior = Seek.Instance;
                    break;
                case eSteeringBehavior.Pursuit :
                    behavior = Pursuit.Instance;
                    break;
                case eSteeringBehavior.PursuitPlayer :
                    behavior = PursuitPlayer.Instance;
                    break;
                case eSteeringBehavior.Catch :
                    behavior = Catch.Instance;
                    break;
                case eSteeringBehavior.Flee :
                    behavior = Flee.Instance;
                    break;
                case eSteeringBehavior.FleePlayer :
                    behavior = FleePlayer.Instance;
                    break;
                case eSteeringBehavior.Hide :
                    behavior = Hide.Instance;
                    break;
                case eSteeringBehavior.HidePlayer :
                    behavior = HidePlayer.Instance;
                    break;
                case eSteeringBehavior.LookAt :
                    behavior = LookAt.Instance;
                    break;
                case eSteeringBehavior.LookAtPlayer :
                    behavior = LookAtPlayer.Instance;
                    break;
            }

            behavior.Enter(owner);

            SteeringBehaviorChanged?.Invoke(owner, value);
        }
    }

    public void SetDestination (Vector3 destination){
        //if(!canComputePath) return;

        NavMeshHit hit;
        if(!NavMesh.SamplePosition(destination, out hit, 30, NavMesh.AllAreas)){
            Debug.LogWarning("Creature Die Because off of navmesh");
            owner.Creature.Die();
            return;
        }
        destination = hit.position;

        Destination = destination;

        if(!nav.isOnNavMesh){
            Debug.LogWarning("Creature Die Because off of navmesh");
            owner.Creature.Die();
            return;
        }

        nav.CalculatePath(Destination, computingPath);

        //owner.StartCoroutine(WaitAndCanComputePath());
    }

    public void NavStop () {
        if(!nav.isOnNavMesh){
            Debug.LogWarning("Creature Die Because off of navmesh");
            owner.Creature.Die();
            return;
        }

        SetDestination(owner.transform.position);
        nav.isStopped = true;
    }

    public void NavStart () {
        if(!nav.isOnNavMesh){
            Debug.LogWarning("Creature Die Because off of navmesh");
            owner.Creature.Die();
            return;
        }

        nav.isStopped = false;
    }

    /*private IEnumerator WaitAndCanComputePath () {
        canComputePath = false;
        yield return new WaitForSeconds(0.1f);
        canComputePath = true;
    }*/

    /*public NavMeshAgent Nav {
        get{
            return nav;
        }
    }*/

    /*public NavMeshPath Path {
        get{
            return nav.path;
        }
        set{
            nav.path = value;
        }
    }*/

    public bool IsArrivedToDestination {
        get{
            if(!nav.isOnNavMesh){
                Debug.LogWarning("Creature Die Because off of navmesh");
                owner.Creature.Die();
                return false;
            }

            return !nav.pathPending && nav.remainingDistance < offsetDist;
        }
    }

    /*public Vector3 NextPos {
        get{
            return owner.transform.position + Velocity * Time.deltaTime * 50;
        }
    }*/
}
