using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIEnemyDamageText : MonoBehaviour, IObserver
    {

        public Character owner;
        private Text damageText;
        private TimerAction textDisappear;
        private float accumulatedDamage = 0f;

        // Use this for initialization
        void Start()
        {
            if (!owner)
                owner = GetComponentInParent<Character>();
            damageText = GetComponent<Text>();
            damageText.enabled = false;
            accumulatedDamage = 0f;
            this.Observe(Message.Combat_HealthChange);
            textDisappear = new TimerAction(this, 2f, () => {
                damageText.enabled = false;
                accumulatedDamage = 0f;
            });
        }

        public void OnNotification(object sender, Message msg, params object[] args)
        {
            bool senderIsOwner = (Object)sender == owner;
            if (!senderIsOwner)
                return;

            switch (msg)
            {
                case Message.Combat_HealthChange:
                    float diff = (float)args[0];
                    accumulatedDamage += Mathf.Abs(diff);
                    damageText.enabled = true;
                    damageText.text = Mathf.CeilToInt(accumulatedDamage).ToString();
                    textDisappear.Start();
                    break;
            }
        }
    }
}