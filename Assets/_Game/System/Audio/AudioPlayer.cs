using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Patterns.Observer;
using Patterns.ObjectPool;
using Sirenix.OdinInspector;

public class AudioPlayer : MonoBehaviour, IObserver
{

    [Range(0, 10)]
    public int sourcePool = 10;
    public SFXMap sfxMap;

    public List<GameObject> senders;
    public List<AudioEvent> events;

    private AudioSource prefab;
    private ObjectPool<AudioSource> audioSources;

    void Start()
    {
        prefab = GetComponentInChildren<AudioSource>();
        audioSources = new ObjectPool<AudioSource>(prefab, sourcePool);
        foreach (var source in audioSources.FetchAll())
        {
            source.transform.position = prefab.transform.position;
            source.transform.parent = prefab.transform.parent;
        }

        foreach (var evt in events)
            this.Observe(evt.message);
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        if (senders.Contains(((Component)sender).gameObject))
        {
            foreach (var evt in events)
            {
                if (evt.message == msg)
                {
                    var source = audioSources.Fetch();
                    var sfx = sfxMap.GetSFX(evt.Effect);
                    if(sfx == null)
                        return;

                    float pitch = sfx.options.pitchVariance > 0f
                        ? Mathf.Clamp(
                            Random.Range(sfx.options.pitch - (sfx.options.pitchVariance / 2f),
                            sfx.options.pitch + (sfx.options.pitchVariance / 2f)), -3f, 3f)
                        : sfx.options.pitch;

                    source.priority = sfx.options.priority;
                    source.volume = sfx.options.volume;
                    source.pitch = pitch;
                    source.panStereo = sfx.options.stereoPan;
                    if (sfx.options.delay <= 0f)
                        source.PlayOneShot(sfx.clip);
                    else
                    {
                        source.clip = sfx.clip;
                        source.PlayDelayed(sfx.options.delay);
                    }
                }
            }
        }
    }

}

[System.Serializable]
public class AudioEvent
{
    public enum ClipChoiceMethod
    {
        RoundRobin,
        Random
    }

    private bool HasMultipleFX {
        get {
            if (effects != null)
                return effects.Length > 1;
            else
                return false;
        }
    }

    public SFXName Effect {
        get {
            if (effects == null)
                return SFXName.SFX_None;
            else if (effects.Length == 0)
                return SFXName.SFX_None;
            else if (effects.Length == 1)
                return effects[0];

            if (choiceMethod == ClipChoiceMethod.RoundRobin)
            {
                rr_currentEffect = (rr_currentEffect >= 0 && rr_currentEffect < effects.Length) ? rr_currentEffect : 0;
                return effects[rr_currentEffect++];
            }
            else if (choiceMethod == ClipChoiceMethod.Random)
                return effects[Random.Range(0, effects.Length)];

            return SFXName.SFX_None;
        }
    }

    public Message message;

    [SerializeField]
    private SFXName[] effects;

    [ShowIf("HasMultipleFX")]
    public ClipChoiceMethod choiceMethod = ClipChoiceMethod.RoundRobin;

    private int rr_currentEffect = 0;


}