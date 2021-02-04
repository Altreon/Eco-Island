using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DNADistortion
{
    private List<Particularity> particularities;

    public DNADistortion () {
        particularities = new List<Particularity>();
    }

    public void Update () {
        UpdateParticularities();
    }

    private void UpdateParticularities () {
        foreach(Particularity particularity in particularities) {
            if(particularity.ActivationCondition()){
                particularity.Activation();
            }else if(particularity.PrepareCondition()){
                particularity.Prepare();
            }else{
                particularity.Inactif();
            }
        }
    }

    public bool AddParticularity (Particularity particularity) {
        if(!particularities.Exists(part => part.GetType() == particularity.GetType())){
            particularities.Add(particularity);
            return true;
        }else{
            return false;
        }
    }

    public bool RemoveParticularity (System.Type type) {
        Particularity particularity = particularities.FirstOrDefault(part => part.GetType() == type);

        if(particularity != null){
            particularities.Remove(particularity);
            return true;
        }else{
            return false;
        }
    }

    public bool HaveParticularity (System.Type type) {
        return particularities.Exists(part => part.GetType() == type);
    }

    public Particularity GetParticularity (System.Type type) {
        return particularities.FirstOrDefault(part => part.GetType() == type);
    }

    public List<ParticularityType> AllParticularityTypes () {
        List<ParticularityType> types = new List<ParticularityType>();

        foreach(Particularity particularity in particularities) {
            ParticularityType type = 0;
            switch(particularity){
                case Explosion e : 
                    type = ParticularityType.Explosion;
                    break;
                case Venom v : 
                    type = ParticularityType.Venom;
                    break;
                case Spark s : 
                    type = ParticularityType.Spark;
                    break;
                case Ghost g : 
                    type = ParticularityType.Ghost;
                    break;
                case Rouli r : 
                    type = ParticularityType.Rouli;
                    break;
                case Vacuum v : 
                    type = ParticularityType.Vacuum;
                    break;
            }

            if(!types.Contains(type)) types.Add(type);
        }

        return types;
    }

    public void DestroyAllParticularities () {
        foreach(Particularity particularity in particularities) {
            particularity.Destroy();
        }
    }
}
