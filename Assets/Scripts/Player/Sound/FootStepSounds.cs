using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Classe : FootStepSounds
Système qui génèrent des bruits de pas selon la vitesse de déplacement du joueur
*/

[RequireComponent(typeof(AudioBox))]
public class FootStepSounds : MonoBehaviour
{
    [SerializeField]
    private SoundOneShot soundWalk = SoundOneShot.PlayerWalk;
    [SerializeField]
    private SoundOneShot soundRun = SoundOneShot.PlayerRun;
    [SerializeField]
    private PlayerController player = null;
    [SerializeField]

    public float FreqWithVelocity = 4;
    public float velocityWalkDelimiter = 3;

    private AudioBox audioBox;
    private float lastFootTime = 0;

    private void Awake () 
    {
        audioBox = GetComponent<AudioBox>();
    }

    void Update() 
    {
        if(!player.IsOnGround) return;

        float velocity = player.Velocity.magnitude;
        float freq = FreqWithVelocity / player.Velocity.magnitude;

        if(Time.time - lastFootTime > freq)
        {
            if(velocity > velocityWalkDelimiter)
            {
                audioBox.PlayOneShot(soundRun);
            }
            else
            {
                audioBox.PlayOneShot(soundWalk, velocity / velocityWalkDelimiter);
            }

            lastFootTime = Time.time;
        }
    }
}
