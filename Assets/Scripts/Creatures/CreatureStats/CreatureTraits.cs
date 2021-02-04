using System;
using System.Collections.Generic;
using UnityEngine;

public enum CREATURE_TRAITS
{
    AGGRESSIVITY,
    GLUTTONY,
    LUST,
    HYSTERESIS,
    SOCIABILTY,
    CURIOSITY,
    
    CARNIVOROUS,
    STRENGTH,
    CONSTITUTION,
    VISION,
    SPEED,
    VIGILANCE,
    NOCTURNAL
    
}

[System.Serializable]
public class CreatureTraits
{
    public event Action<Trait> UpdatedTrait;

    public void notifyTraitChanged(Trait changed)
    {
        if (UpdatedTrait != null)
            UpdatedTrait(changed);
    }

    private void CreatureTraitsInit()
    {
        _aggressivity = new Trait(this);
        _carnivorous = new Trait(this);
        _constitution = new Trait(this);
        _gluttony = new Trait(this);
        _lust = new Trait(this);
        _hysteresis = new Trait(this);
        _sociability = new Trait(this);
        _curiosity = new Trait(this);
        _strength = new Trait(this);
        _vision = new Trait(this);
        _speed = new Trait(this);
        _vigilance = new Trait(this);
        _nocturnal = new Trait(this);
    }
    public CreatureTraits(CreatureTraits toCopy)
    {
        /* Je penserais pas que les pointeurs me manquerait un jour
        foreach (CREATURE_TRAITS trait in (CREATURE_TRAITS[]) System.Enum.GetValues(typeof(CREATURE_TRAITS)))
        {
            Get(trait) = new Trait(); //RIP
        }
        */
        CreatureTraitsInit();
        //_aggressivity = new Trait(this);
        _aggressivity.Value = toCopy._aggressivity.Value;
        //_carnivorous = new Trait(this);
        _carnivorous.Value = toCopy._carnivorous.Value;
        //_constitution = new Trait(this);
        _constitution.Value = toCopy._constitution.Value;
        //_gluttony = new Trait(this);
        _gluttony.Value = toCopy._gluttony.Value;
        //_lust = new Trait(this);
        _lust.Value = toCopy._lust.Value;
        //_hysteresis = new Trait(this);
        _hysteresis.Value = toCopy._hysteresis.Value;
        //_sociability = new Trait(this);
        _sociability.Value = toCopy._sociability.Value;
        //_curiosity = new Trait(this);
        _curiosity.Value = toCopy._curiosity.Value;
        //_strength = new Trait(this);
        _strength.Value = toCopy._strength.Value;
        //_vision = new Trait(this);
        _vision.Value = toCopy._vision.Value;
        //_speed = new Trait(this);
        _speed.Value = toCopy._speed.Value;
        //_vigilance = new Trait(this);
        _vigilance.Value = toCopy._vigilance.Value;
        _nocturnal.Value = toCopy._nocturnal.Value;

    }

    //Si quelqu'un regarde ici et n'est pas d'accord, c'est moi Jirachi, le responsable :eyes:
    public CreatureTraits(
        float aggressivity,
        float carnivorous,
        float constitution,
        float gluttony,
        float lust,
        float hysteresis,
        float sociability,
        float curiosity,
        float strength,
        float vision,
        float speed,
        float vigilance,
        float nocturnal
        )
    {
        CreatureTraitsInit();
        _aggressivity.Value = aggressivity;
        _carnivorous.Value = carnivorous;
        _constitution.Value = constitution;
        _gluttony.Value = gluttony;
        _lust.Value = lust;
        _hysteresis.Value = hysteresis;
        _sociability.Value = sociability;
        _curiosity.Value = curiosity;
        _strength.Value = strength;
        _vision.Value = vision;
        _speed.Value = speed;
        _vigilance.Value = vigilance;
        _nocturnal.Value = nocturnal;
    }
    
    [SerializeField] private Trait _aggressivity;
    public Trait Aggressivity
    {
        get { return _aggressivity;}
        private set
        {
            _aggressivity = value;
        }
    }
    
    [SerializeField] private Trait _gluttony;
    public Trait Gluttony
    {
        get { return _gluttony;}
        private set { _gluttony = value;
        }
    }

    [SerializeField] private Trait _lust;
    public Trait Lust
    {
        get { return _lust;}
        private set { _lust = value; 
        }
    }

    [SerializeField] private Trait _hysteresis;
    public Trait Hysteresis
    {
        get { return _hysteresis;}
        private set { _hysteresis = value; 
        }
    }

    [SerializeField] private Trait _sociability;
    public Trait Sociability
    {
        get { return _sociability;}
        private set { _sociability = value; 
        }
    }

    [SerializeField] private Trait _curiosity;
    public Trait Curiosity
    {
        get { return _curiosity;}
        private set { _curiosity = value; 
        }
    }
    [SerializeField] private Trait _strength;
    public Trait Strength
    {
        get { return _strength;}
        private set { _strength = value; 
        }
    }
    [SerializeField] private Trait _vision;
    public Trait Vision
    {
        get { return _vision;}
        private set { _vision = value; 
        }
    }
    [SerializeField] private Trait _speed;
    public Trait Speed
    {
        get { return _speed;}
        private set { _speed = value; 
        }
    }
    [SerializeField] private Trait _vigilance;
    public Trait Vigilance
    {
        get { return _vigilance;}
        private set { _vigilance = value; 
        }
    }
    [SerializeField] private Trait _constitution;
    public Trait Constitution
    {
        get { return _constitution;}
        private set { _constitution = value; 
        }
    }

    [SerializeField] private Trait _carnivorous;
    public Trait Carnivorous
    {
        get { return _carnivorous;}
        private set { _carnivorous = value; 
        }
    }

    [SerializeField] private Trait _nocturnal;
    public Trait Nocturnal
    {
        get { return _nocturnal;}
        private set { _nocturnal = value; 
        }
    }

    public Trait Get(CREATURE_TRAITS value)
    {
        switch (value)
        {
            case CREATURE_TRAITS.CARNIVOROUS: //Rouge / Verte
                return Carnivorous;
            case CREATURE_TRAITS.AGGRESSIVITY: //Bleu clair / Bleu foncé / Rose
                return Aggressivity;
            case CREATURE_TRAITS.LUST: // + S
                return Lust;
            case CREATURE_TRAITS.SPEED: // + S
                return Speed;
            case CREATURE_TRAITS.VISION: // - S
                return Vision;
            case CREATURE_TRAITS.GLUTTONY: // - S
                return Gluttony;
            case CREATURE_TRAITS.STRENGTH: // V aussi
                return Strength;
            case CREATURE_TRAITS.CURIOSITY: // + S
                return Curiosity;
            case CREATURE_TRAITS.VIGILANCE: // - S
                return Vigilance;
            case CREATURE_TRAITS.HYSTERESIS: // - S
                return Hysteresis;
            case CREATURE_TRAITS.SOCIABILTY: // + S
                return Sociability;
            case CREATURE_TRAITS.NOCTURNAL:
                return Nocturnal;
            default:
                return Constitution;
        }
    }

    public override string ToString()
    {
        String s = "Traits={";
        foreach (CREATURE_TRAITS t in (CREATURE_TRAITS[])Enum.GetValues(typeof(CREATURE_TRAITS)))
        {
            s += t.ToString() + "=" + Get(t).Value + ";";
        }
        return s + "}";
    }

    public static Color GetColor(CreatureTraits traits)
    {
        float minH = 0f;
        float maxH = Mathf.InverseLerp(0f, 360f, 320f); //~ 0.85f : Sinon le rouge est 2 fois
        //float maxH = 1;
        float minV = 0.75f; //Sinon c'est trop sombre
        float maxV = 1f;
        float minS = 0.65f; //Sinon c'est trop fade
        float maxS = 1f;

        float H = -1f;
        float S = -1f;
        float V = -1f;
        
        //Sans révision des couleurs
        float extremeAggre = Mathf.Abs(traits._aggressivity - 0.5f); //0.5 == ++Extreme ; 0 == pas ectreme
        float extremeCarn = Mathf.Abs(traits._carnivorous - 0.5f);
        if (extremeAggre > extremeCarn)
        {
            //bleu clair / bleu foncé / rose
            H = Mathf.Lerp((minH + maxH) / 2f, maxH, traits._carnivorous);
        }
        else
        {
            //Rouge / vert
            H = Mathf.Lerp(minH, (minH + maxH) / 2f, 1f - traits._carnivorous);
        }

        float thresholdV = (maxV - minV) / 3f;
        float addedV = Mathf.Lerp(0f, thresholdV, traits._aggressivity);
        addedV += Mathf.Lerp(0f, thresholdV, traits.Strength);
        addedV += Mathf.Lerp(0f, thresholdV, traits._constitution);
        V = maxV - addedV;
        S = (minS + maxS) / 2f;
        float thresholdS = (maxS - S) / 4f;
        S += Mathf.Lerp(0f, thresholdS, traits._lust);
        S += Mathf.Lerp(0f, thresholdS, traits._speed);
        S += Mathf.Lerp(0f, thresholdS, traits._curiosity);
        S += Mathf.Lerp(0f, thresholdS, traits._sociability);
        S -= Mathf.Lerp(0f, thresholdS, traits._vision);
        S -= Mathf.Lerp(0f, thresholdS, traits._gluttony);
        S -= Mathf.Lerp(0f, thresholdS, traits._vigilance);
        S -= Mathf.Lerp(0f, thresholdS, traits._hysteresis);
        //TODO Nocturnal
        return UnityEngine.Color.HSVToRGB(H, S, V);
    }
}
