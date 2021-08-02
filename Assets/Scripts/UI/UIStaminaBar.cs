using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIStaminaBar : MonoBehaviour, IObserver
    {

        public Player player;
        //private UIProgressBar staminaBar;

        // Use this for initialization
        void Start ()
        {
            //staminaBar = GetComponent<UIProgressBar>();
            this.Observe(Message.Combat_StaminaChange);
        }

        public void OnNotification(object sender, Message msg, params object[] args)
        {
            bool senderIsPlayer = (Object)sender == player;
            if (!senderIsPlayer)
                return;

            switch(msg)
            {
                case Message.Combat_StaminaChange:
                    float diff = (float)args[0];
                    //staminaBar.fillAmount = player.Stamina.Value / player.Stamina.maxValue;
                    break;
            }
        }
    }
}