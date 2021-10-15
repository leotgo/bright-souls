using UnityEngine;

namespace BrightSouls
{
    public class PlayerCombatEvents
    {
        /* --------------------------------- Events --------------------------------- */

        public event System.Action onStagger;
        public event System.Action onTakeDamage;
        public event System.Action onBlockHit;
        public event System.Action onBlockBroken;
        public event System.Action<Vector3> onDodge;

        /* ------------------------------ Event Raisers ----------------------------- */

        public void RaiseOnStaggerEvent()
        {
            onStagger.Invoke();
        }

        public void RaiseOnTakeDamageEvent()
        {
            onTakeDamage.Invoke();
        }

        public void RaiseOnBlockHitEvent()
        {
            onBlockHit.Invoke();
        }

        public void RaiseOnBlockBrokenEvent()
        {
            onBlockBroken.Invoke();
        }

        public void RaiseOnDodgeExecutedEvent(Vector3 dir)
        {
            onDodge.Invoke(dir);
        }

        /* -------------------------------------------------------------------------- */
    }
}