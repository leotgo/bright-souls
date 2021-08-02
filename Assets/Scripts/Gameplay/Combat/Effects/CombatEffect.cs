using UnityEngine;
using System.Collections;

namespace BrightSouls
{
    public abstract class CombatEffect
    {
        public abstract void Apply(Character target);
    }

    public abstract class InstantEffect : CombatEffect { /* supposed to be empty */ }

    public abstract class OvertimeEffect : CombatEffect
    {
        [SerializeField] private float tickTime;
        [SerializeField] private float duration;

        public override void Apply(Character target)
        {
            target.StartCoroutine(UpdateRoutine());
        }

        public IEnumerator UpdateRoutine()
        {
            float time = 0f;
            float tick = 0f;
            while(time < duration)
            {
                if(tick > tickTime)
                {
                    Tick();
                    tick = 0f;
                }
                tick += Time.fixedDeltaTime;
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            time = 0f;
        }

        public abstract void Tick();
    }

    public class DamageHealth : InstantEffect
    {
        [SerializeField] private float damageAmount;

        public override void Apply(Character target)
        {
        }
    }
}