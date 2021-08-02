using UnityEngine;
using Patterns.Observer;

namespace BrightSouls.Audio
{
    public enum ClipChoiceMethod
    {
        RoundRobin,
        Random
    }

    [System.Serializable]
    public class AudioEvent
    {
        public SFXName Effect
        {
            get
            {
                if (effects == null)
                {
                    return SFXName.SFX_None;
                }
                else if (effects.Length == 0)
                {
                    return SFXName.SFX_None;
                }
                else if (effects.Length == 1)
                {
                    return effects[0];
                }

                if (choiceMethod == ClipChoiceMethod.RoundRobin)
                {
                    rr_currentEffect = (rr_currentEffect >= 0 && rr_currentEffect < effects.Length) ? rr_currentEffect : 0;
                    return effects[rr_currentEffect++];
                }
                else if (choiceMethod == ClipChoiceMethod.Random)
                {
                    return effects[Random.Range(0, effects.Length)];
                }

                return SFXName.SFX_None;
            }
        }

        private bool HasMultipleFX
        {
            get
            {
                if (effects != null)
                {
                    return effects.Length > 1;
                }
                else
                {
                    return false;
                }
            }
        }

        public Message message;
        [SerializeField]
        private SFXName[] effects;
        public ClipChoiceMethod choiceMethod = ClipChoiceMethod.RoundRobin;
        private int rr_currentEffect = 0;
    }
}