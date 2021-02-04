using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.InputSystem;

/**
Classe : Player
Classe représentant le joueur dans le monde du jeu
*/

public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}
    public PlayerConfig PlayerPreset;

    public PlayerController PlayerController {get; private set;}
    public PlayerHealth PlayerHealth {get; private set;}
    public PlayerPickAndDrop PlayerPickAndDrop {get; private set;}

    public bool IsDie {get; private set;} = false;

    static float t = 0.0f;
    private float deadAnimationTime;

    private GameObject[] SpawnPoints;

    private void Awake () 
    {
         if(Instance){
            Destroy(this);
            return;
        }

        Instance = this;

        PlayerController = GetComponentInChildren<PlayerController>();
        PlayerHealth = GetComponentInChildren<PlayerHealth>();
        PlayerPickAndDrop = GetComponentInChildren<PlayerPickAndDrop>();
    }

    private void Start() 
    {
        this.deadAnimationTime = PlayerPreset.deadAnimationTime;

        gameObject.transform.parent = null;
        PlayerController.transform.parent = null;
        gameObject.transform.SetParent(PlayerController.transform);

        PlayerController.gameObject.SetActive(true);
        PlayerController.enabled = true;

        InputManager.Input.PlayerGhost.Enable();

        CameraController cam = CameraController.Instance;
        cam.Target = PlayerController.gameObject;

        PlayerHealth.healthChanged += HealthChanged;

        SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    private void HealthChanged (float health) 
    {
        if(health == 0) 
        {
            Die();
        }
    }

    public void Die()
    {
        if(PlayerPickAndDrop.IsHandleFood)
        {
            PlayerPickAndDrop.DropOffFood();
        }

        PlayerHealth.enabled = false;   
        PlayerController.enabled = false;    
        
        VenomEffect venom = GetComponent<VenomEffect>();
        if(venom) Destroy(venom);
        ScreenshotHandler.instance.DeathPlayer();
        IsDie = true;

        CameraController.Instance.CanControll = false;
        
    }

    private void Update() 
    {
        if(IsDie)
        {
            DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(0.4f, 1.0f,t);
            DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(0.4f, 1.0f,t);
            DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<ColorGrading>().postExposure.value = Mathf.Lerp(1f, -5f,t);
            DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<ColorGrading>().postExposure.value = Mathf.Lerp(1f, -5f,t);
            t += 1/deadAnimationTime * Time.deltaTime;

            if (t > 1.0f)
            {
                t = 0.0f;
                IsDie = false;

                Respawn();
            }
        }
        else
        {
            float healthPercent = 1 - PlayerHealth.Health / PlayerHealth.MaxHealth;

            DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(0.4f, 1.0f, healthPercent);
            DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<Vignette>().intensity.value = Mathf.Lerp(0.4f, 1.0f, healthPercent);
            DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<Vignette>().color.value = Vector4.Lerp(Color.black, Color.red, healthPercent);
            DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<Vignette>().color.value = Vector4.Lerp(Color.black, Color.red, healthPercent);
        }
    }

    private void Respawn()
    {
        DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<Vignette>().intensity.value = 0.4f;
        DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<Vignette>().intensity.value = 0.4f;
        DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<Vignette>().color.value = Color.black;
        DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<Vignette>().color.value = Color.black;

        DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<ColorGrading>().postExposure.value = 1.0f;
        DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<ColorGrading>().postExposure.value = 1.0f;

        PlayerController.ControllerActive = false;

        int index = Random.Range(0,SpawnPoints.Length);
        PlayerController.gameObject.transform.position = SpawnPoints[index].transform.position;

        PlayerController.ControllerActive = true;

        PlayerHealth.Health = PlayerHealth.MaxHealth;

        PlayerHealth.enabled = true;   
        PlayerController.enabled = true;  

        CameraController.Instance.CanControll = true;
    }

    private void OnDestroy () 
    {
        if(Instance == this) Instance = null;
    }
}
