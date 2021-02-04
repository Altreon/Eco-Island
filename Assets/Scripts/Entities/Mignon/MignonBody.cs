using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MignonBody : MonoBehaviour
{
    [SerializeField]
    Transform[] bodies = null;
    [SerializeField]
    float jigleSpeedActif = 5;
    [SerializeField]
    float jigleSpeedSleep = 2;

    public bool JigleSleep {get; set;} = false;

    private float randTime;

    private Renderer rend;
    private  Vector3[] initPos;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        randTime = Random.Range(0, jigleSpeedActif);

        initPos = new Vector3[bodies.Length];

        for (int i = 0; i < bodies.Length; ++i){
            initPos[i] = bodies[i].localPosition;
        }
    }

    private void Update()
    {
        float time = Time.time * (JigleSleep ? jigleSpeedSleep : jigleSpeedActif) + randTime;

        rend.material.SetFloat("_Progression", time);

        for (int i = 0; i < bodies.Length; ++i){
            Vector3 pos = new Vector3(Mathf.Sin(time)/50f, Mathf.Cos(time)/50f, 0);
            if(initPos[i].x < 0) pos.x *= -1;
            if(initPos[i].y < 0) pos.y *= -1;
            bodies[i].localPosition = initPos[i] + pos;
        }
    }
}
