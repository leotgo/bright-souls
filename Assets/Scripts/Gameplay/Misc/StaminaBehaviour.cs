using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using BrightSouls.Gameplay;

namespace BrightSouls
{
    public class StaminaBehaviour : MonoBehaviour
    {
        public float Value
        {
            get => _value;
            set
            {
                float lastStamina = _value;
                _value = Mathf.Clamp(value, 0f, maxValue);
                float diff = _value - lastStamina;
                if (diff < 0f)
                {
                    staminaDecreased = true;
                    isRecovering = false;
                    recoverDelayTime = 0f;
                }
                player.Notify(Message.Combat_StaminaChange, diff);
            }
        }

        /* -------------------------- Component References -------------------------- */

        [SerializeField] private Player player;

        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private float recoverAmount = 18f;
        [SerializeField] private float recoverDelay = 2f;
        [SerializeField] private float blockRecoverModifier = 0.35f;
        [SerializeField] private float maxValue = 100f;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private float _value = 100f;
        private float recoverDelayTime = 0f;
        private float bonusRecover = 0f;
        private bool isRecovering = false;
        private bool staminaDecreased = false;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            isRecovering = false;
        }

        private void FixedUpdate()
        {
            if (staminaDecreased && !isRecovering)
            {
                recoverDelayTime += Time.fixedDeltaTime;
                if (recoverDelayTime >= recoverDelay)
                {
                    staminaDecreased = false;
                    isRecovering = true;
                    recoverDelayTime = 0f;
                }
            }
            else if (isRecovering)
            {
                if (player.State.IsStaggered)
                {
                    staminaDecreased = true;
                    isRecovering = false;
                    recoverDelayTime = 0f;
                }
                float blockModifier = player.State.IsBlocking ? blockRecoverModifier : 1f;
                Value += blockModifier * (recoverAmount + bonusRecover) * Time.fixedDeltaTime;
                if(Value >= maxValue)
                {
                    staminaDecreased = false;
                    isRecovering = false;
                    recoverDelayTime = 0f;
                }
            }
        }

        /* -------------------------------------------------------------------------- */
    }
}