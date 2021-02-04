using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
Classe : PlayerHealth
Gère la santé du joueur
*/

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth;
    public float TimeToRecover;
    public float RecoverSpeed;

    private float health;
    [SerializeField]
    private float DebugHealh;

    private float lastDamageTime = 0;

    public Action<float> healthChanged;

    public float Health
    {
        get{
            return health;
        }

        set{
            if(health != value)
            {
                if(health > value)
                {
                    lastDamageTime = Time.time;
                }

                health = Mathf.Clamp(value,0, MaxHealth);
                DebugHealh = health;
                healthChanged?.Invoke(health);
            }
        }
    }

    private void Awake () 
    {
        this.MaxHealth = Player.Instance.PlayerPreset.MaxHealth;
        this.TimeToRecover = Player.Instance.PlayerPreset.TimeToRecover;
        this.RecoverSpeed = Player.Instance.PlayerPreset.RecoverSpeed;

        Health = MaxHealth;
    }

    private void Update () 
    {
        if(Health < MaxHealth && Time.time - lastDamageTime > TimeToRecover) {
            Health += RecoverSpeed * Time.deltaTime;
        }
    }
}
