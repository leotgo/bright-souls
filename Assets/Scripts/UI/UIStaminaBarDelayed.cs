using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;
using BrightSouls.Gameplay;

namespace BrightSouls.UI
{
    public class UIStaminaBarDelayed : MonoBehaviour
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

        private void Start()
        {
            owner.Stamina.onAttributeChanged += OnStaminaChanged;
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
                bar.fillAmount = Mathf.SmoothStep(bar.fillAmount, owner.Stamina.Value / owner.MaxStamina.Value, updateTime / updateDuration);
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

        /* ------------------------- Attribute Change Events ------------------------ */

        private void OnStaminaChanged(float oldValue, float newValue)
        {
            if (oldValue > newValue)
            {
                ResetBarDelay();
            }
            else
            {
                bar.fillAmount = owner.Stamina.Value / owner.MaxStamina.Value;
            }
        }

        /* --------------------------------- Helpers -------------------------------- */

        private void ResetBarDelay()
        {
            updateTime = 0f;
            updateDelayTime = 0f;
            isWaitingUpdate = true;
            isUpdating = false;
        }

        /* -------------------------------------------------------------------------- */
    }
}