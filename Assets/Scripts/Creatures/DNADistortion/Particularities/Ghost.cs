using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Particularity
{
    private GhostConfig config;

    private float lastTimeDoing = 0;

    public Ghost (Creature owner, GhostConfig config) : base(owner) {
        this.config = config;
    }

    public override bool PrepareCondition () {
        return false;
    }

    public override void Prepare () {
       
    }

    public void Appear () {
        lastTimeDoing = Time.time;
    }

    public override bool ActivationCondition () {
        return Time.time - lastTimeDoing > config.appearDuration;
    }

    public override void Activation () {
        owner.ColorSwap.SetTransparency(Mathf.Lerp(owner.ColorSwap.GetTransparency(), config.transparency, config.transitionSpeed * Time.deltaTime));
    }

    public override void Inactif () {
        owner.ColorSwap.SetTransparency(Mathf.Lerp(owner.ColorSwap.GetTransparency(), 1, config.transitionSpeed * Time.deltaTime));
    }

    public override void Destroy () {
        
    }
}
