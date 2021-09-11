using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIEnemyDamageText : MonoBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private ICombatCharacter owner;
        [SerializeField] private Text damageText;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private TimerAction textDisappear;
        private float accumulatedDamage = 0f;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            damageText.enabled = false;
            accumulatedDamage = 0f;
            textDisappear = new TimerAction(this, 2f, () => {
                damageText.enabled = false;
                accumulatedDamage = 0f;
            });
            owner.Health.onAttributeChanged += OnHealthChanged;
        }

        /* ----------------------------- Event Callbacks ---------------------------- */

        private void OnHealthChanged(float oldValue, float newValue)
        {
            float healthDiff = newValue - oldValue;
            accumulatedDamage += Mathf.Abs(healthDiff);
            damageText.enabled = true;
            damageText.text = Mathf.CeilToInt(accumulatedDamage).ToString();
            textDisappear.Start();
        }

        /* -------------------------------------------------------------------------- */
    }
}