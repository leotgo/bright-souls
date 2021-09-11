using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIHealthBarDelayed : MonoBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private ICombatCharacter owner;
        [SerializeField] private Image bar;
        [SerializeField] private float updateDelay = 1f;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private float updateDelayTime = 0f;
        private bool isWaitingUpdate = false;
        private float updateTime = 0f;
        private float updateDuration = 2f;
        private bool isUpdating;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start ()
        {
            isUpdating = false;
            owner.Health.onAttributeChanged += OnHealthChanged;
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
                bar.fillAmount = Mathf.SmoothStep(bar.fillAmount, owner.Health.Value / owner.MaxHealth.Value, updateTime / updateDuration);
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

        /* ----------------------- Attribute Change Callbacks ----------------------- */

        private void OnHealthChanged(float oldValue, float newValue)
        {
            if (oldValue > newValue)
            {
                updateTime = 0f;
                updateDelayTime = 0f;
                isWaitingUpdate = true;
                isUpdating = false;
            }
            else
            {
                bar.fillAmount = owner.Health.Value / owner.MaxHealth.Value;
            }
        }

        /* -------------------------------------------------------------------------- */
    }
}