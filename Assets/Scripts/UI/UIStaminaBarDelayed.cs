using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIStaminaBarDelayed : MonoBehaviour, IObserver
    {

        public Player player;


        public float updateDelay = 1f;
        private float updateDelayTime = 0f;
        private bool isWaitingUpdate = false;

        private float updateTime = 0f;
        private float updateDuration = 2f;
        private bool isUpdating;

        private Image bar;


        private void Start()
        {
            bar = GetComponent<Image>();
            this.Observe(Message.Combat_StaminaChange);
            isUpdating = false;
        }

        private void Update()
        {
            if (isUpdating)
            {
                updateTime += Time.deltaTime;
                if (updateTime > updateDuration)
                {
                    updateTime = updateDuration;
                    isUpdating = false;
                }
                bar.fillAmount = Mathf.SmoothStep(bar.fillAmount, (float)player.Stamina.Value / (float)player.Stamina.maxValue, updateTime / updateDuration);
            }
            else
            {
                if (isWaitingUpdate)
                {
                    updateDelayTime += Time.deltaTime;
                    if (updateDelayTime > updateDelay)
                    {
                        isUpdating = true;
                        isWaitingUpdate = false;
                    }
                }
            }
        }

        public void OnNotification(object sender, Message msg, params object[] args)
        {
            bool senderIsPlayer = (Object)sender == player;
            if (!senderIsPlayer)
                return;

            switch (msg)
            {
                case Message.Combat_StaminaChange:
                    float diff = (float)args[0];
                    if(diff < 0f)
                    {
                        updateTime = 0f;
                        updateDelayTime = 0f;
                        isWaitingUpdate = true;
                        isUpdating = false;
                    }
                    else
                    {
                        bar.fillAmount = player.Stamina.Value / player.Stamina.maxValue;
                    }

                    break;
            }
        }
    }
}