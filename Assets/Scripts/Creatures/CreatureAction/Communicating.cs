using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicating
{
    private static Communicating _instance;
    public static Communicating Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Communicating();
            }
            return _instance;
        }
    }

    public void Communicate(Creature sender, Creature receiver, MemoryType comType, Action callback = null, float duration = 3)
    {
        Discussion discussion = new Discussion(sender, receiver, comType, callback, duration);
        sender.CreatureDoing.CurrentDiscussion = discussion;
        receiver.CreatureDoing.CurrentDiscussion = discussion;
    }
}

public class Discussion
{
    public Creature sender;
    public Creature receiver;

    private MemoryType comType;
    private float duration;

    private float startTime;

    private Coroutine coroutine;
    private GameObject textMessage;
    private GameObject textMessage2;
    private Action callback;

    public Discussion(Creature sender, Creature receiver, MemoryType comType, Action callback, float duration)
    {
        this.sender = sender;
        this.receiver = receiver;

        this.duration = duration;
        startTime = Time.time;

        this.callback = callback;
        this.comType = comType;
        coroutine = sender.StartCoroutine(CommunicationCoroutine());
    }

    public void Stop()
    {
        sender.StopCoroutine(coroutine);
        //Debug.LogWarning("Communication interrompue");

        MergeBoth(Mathf.Min((Time.time - startTime)/duration, 1));

        Destroy();
    }

    private void Destroy()
    {
        //if (textMessage != null) UnityEngine.Object.Destroy(textMessage);
        if (textMessage != null) ParticuleManager.Instance.DestroyParticle(ParticleType.TypingMessage, textMessage);
        //if (textMessage2 != null) UnityEngine.Object.Destroy(textMessage2);
        if (textMessage2 != null) ParticuleManager.Instance.DestroyParticle(ParticleType.TypingMessage, textMessage2);
        sender.CreatureDoing.CurrentDiscussion = null;
        receiver.CreatureDoing.CurrentDiscussion = null;
    }

    private IEnumerator CommunicationCoroutine()
    {
        Vector3 pos = sender.transform.position + 0.5f * sender.transform.forward + new Vector3(0, 0.5f, 0);
        //textMessage = UnityEngine.Object.Instantiate(ParticuleManager.Instance.TypingMessage, pos, Quaternion.identity);
        textMessage = ParticuleManager.Instance.CreateParticle(ParticleType.TypingMessage, pos, Quaternion.identity);
        textMessage.transform.localScale *= 0.5f;

        textMessage.transform.parent = sender.transform;

        Vector3 pos2 = receiver.transform.position + 0.5f * receiver.transform.forward + new Vector3(0, 0.5f, 0);
        //textMessage2 = UnityEngine.Object.Instantiate(ParticuleManager.Instance.TypingMessage, pos2, Quaternion.identity);
        textMessage2 = ParticuleManager.Instance.CreateParticle(ParticleType.TypingMessage, pos2, Quaternion.identity);
        textMessage2.transform.localScale *= 0.5f;

        textMessage2.transform.parent = receiver.transform;

        //yield return new WaitForSeconds(duration);
        float time = 0;
        while(time < duration){
            if(sender.DNADistortion.HaveParticularity(typeof(Ghost))){
                (sender.DNADistortion.GetParticularity(typeof(Ghost)) as Ghost).Appear();
            }
            if(receiver.DNADistortion.HaveParticularity(typeof(Ghost))){
                (receiver.DNADistortion.GetParticularity(typeof(Ghost)) as Ghost).Appear();
            }

            time += Time.deltaTime;
            yield return null;
        }

        MergeBoth();

        callback?.Invoke();

        Destroy();
    }

    private void MergeBoth(float completion = 1)
    {
        MergeSenderToReceiver(completion);
        MergeReceiverToSender(completion);

        if(completion == 1){
            receiver.AudioBox.PlayOneShot(SoundOneShot.CreatureHappy);
        }
    }

    private void MergeSenderToReceiver(float completion = 1)
    {
        if (receiver.agentCreature != null && sender.agentCreature != null)
        {
            receiver.agentCreature.Memory.MergeFrom(sender.agentCreature, comType, completion);
            receiver.agentCreature.Memory.Communications.Write(new DataCommunication(sender, comType));
        }
    }
    private void MergeReceiverToSender(float completion = 1)
    {
        if (receiver.agentCreature != null && sender.agentCreature != null)
        {
            sender.agentCreature.Memory.MergeFrom(receiver.agentCreature, comType, completion);
            sender.agentCreature.Memory.Communications.Write(new DataCommunication(receiver, comType));
        }
    }
}
