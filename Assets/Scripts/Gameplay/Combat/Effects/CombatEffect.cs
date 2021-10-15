using UnityEngine;
using System.Collections;

namespace BrightSouls
{
    public interface ICombatEffect
    {
        void Apply(ICombatCharacter target);
    }

    public abstract class OvertimeEffect : ICombatEffect
    {
        [SerializeField] private float tickTime;
        [SerializeField] private float duration;

        public void Apply(ICombatCharacter target)
        {
            // TODO add a global monobehaviour to take care of routines for Overtime Effects
            //mono.StartCoroutine(UpdateRoutine());
        }

        private IEnumerator UpdateRoutine(ICombatCharacter target)
        {
            float time = 0f;
            float tick = 0f;
            while(time < duration)
            {
                if(tick > tickTime)
                {
                    OnEffectTick(target);
                    tick = 0f;
                }
                tick += Time.fixedDeltaTime;
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            time = 0f;
        }

        protected abstract void OnEffectTick(ICombatCharacter target);
    }

    public class DamageHealth : ICombatEffect
    {
        [SerializeField] private float damageAmount;

        public void Apply(ICombatCharacter target)
        {
            target.Health.Value -= damageAmount;
        }
    }
}