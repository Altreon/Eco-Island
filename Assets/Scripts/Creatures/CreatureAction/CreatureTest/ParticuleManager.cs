using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : ParticleManager
Factory prenant en charge la création d'effets de particules dans le monde du jeu
*/

public enum ParticleType { 
    BulletExplosionFire = 0,
    MagicExplosionBlue = 1,
    TypingMessage = 2,
    Death = 3,
    StunStars = 4,
    ParticularityFuse = 5,
    ParticularityExplosion = 6,
    ParticularityVenomCreature = 7,
    ParticularityVenomPlayer = 8,
    ParticularityCharge = 9,
    ParticularitySpark = 10,
    ParticularityVacuum = 11,
    FoodDesapear = 12
};
public class ParticuleManager : MonoBehaviour
{
    [SerializeField] private ParticulePool[] ListParticle = null;

    private Stack<GameObject>[] PooledParticle;

    private GameObject ParticleHolder;

    /*#region TYPES
    [SerializeField] private GameObject bulletExplosionFire;
    public GameObject BulletExplosionFire{ get { return bulletExplosionFire; } }

    [SerializeField] private GameObject magicExplosionBlue;
    public GameObject MagicExplosionBlue { get { return magicExplosionBlue; } }

    [SerializeField] private GameObject typingMessage;
    public GameObject TypingMessage { get { return typingMessage; } }
    #endregion*/


    public static ParticuleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        /*//TEMPORAIRE pour tester que tout fonctionne
        if (bulletExplosionFire != null && magicExplosionBlue != null && typingMessage != null)
        {
            ListParticlePrefab = new GameObject[] { bulletExplosionFire, magicExplosionBlue, typingMessage };
        }
        */
        PooledParticle = new Stack<GameObject>[ListParticle.Length];
        for (int i=0; i<ListParticle.Length; i++)
        {
            PooledParticle[i] = new Stack<GameObject>();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        ParticleHolder = new GameObject("Particle Holder");

        for (int i=0; i<ListParticle.Length; i++){
            Stack<GameObject> PooledSpecificParticle = PooledParticle[i];
            GameObject prefab = ListParticle[i].prefab;
            for (int j = 0; j < ListParticle[i].amountToPool; ++j) {
                GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, ParticleHolder.transform);
                obj.SetActive(false); 
                PooledSpecificParticle.Push(obj);
            }
        }
    }

    public GameObject CreateParticle(ParticleType type, Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        Stack<GameObject> PooledSpecificParticle = PooledParticle[(int)type];
        if (PooledSpecificParticle.Count != 0)
        {
            obj = PooledSpecificParticle.Pop();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        else
        {
            obj = Instantiate(ListParticle[(int)type].prefab, position, rotation, ParticleHolder.transform);
        }
        obj.SetActive(true);
        return obj;
    }

    public void DestroyParticle(ParticleType type, GameObject obj)
    {   
        if(!obj){
            return;
        }
        
        PooledParticle[(int)type].Push(obj);
        obj.transform.parent = ParticleHolder.transform;
        obj.transform.localScale = Vector3.one;
        obj.SetActive(false);
    }
}

[System.Serializable]
public class ParticulePool {
    public GameObject prefab;
    [Min(0)]
    public int amountToPool = 0;
}
