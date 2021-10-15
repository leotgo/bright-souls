using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using BrightSouls.Player;

namespace BrightSouls
{
    public class StaminaBehaviour : MonoBehaviour
    {
        private PlayerComponentIndex player;

        [SerializeField] private float recoverAmount = 18f;
        private float bonusRecover = 0f;
        [SerializeField] private float recoverDelay = 2f;

        private float blockRecoverModifier = 0.35f;
        private float recoverDelayTime = 0f;

        public float maxValue = 100f;
        private float _value = 100f;
        public float Value {
            get {
                return _value;
            }
            set {
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

        private bool isRecovering = false;
        private bool staminaDecreased = false;

        private void Start()
        {
            this.player = GetComponent<PlayerComponentIndex>();
            isRecovering = false;
        }

        private void Update()
        {
            if (staminaDecreased && !isRecovering)
            {
                recoverDelayTime += Time.deltaTime;
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
                Value += blockModifier * (recoverAmount + bonusRecover) * Time.deltaTime;
                if(Value >= maxValue)
                {
                    staminaDecreased = false;
                    isRecovering = false;
                    recoverDelayTime = 0f;
                }
            }
        }


    }
}