using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIHealthBar : MonoBehaviour, IObserver
    {

        public Character owner;
        //private UIProgressBar healthBar;

        // Use this for initialization
        void Start ()
        {
            //healthBar = GetComponent<UIProgressBar>();
            this.Observe(Message.Combat_HealthChange);
        }

        public void OnNotification(object sender, Message msg, params object[] args)
        {
            bool senderIsPlayer = (Object)sender == owner;
            if (!senderIsPlayer)
                return;

            switch(msg)
            {
                case Message.Combat_HealthChange:
                    float diff = (float)args[0];
                    //healthBar.fillAmount = owner.Health / owner.maxHealth;
                    break;
            }
        }
    }
}