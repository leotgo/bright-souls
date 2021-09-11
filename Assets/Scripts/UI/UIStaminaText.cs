using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIStaminaText : MonoBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private ICombatCharacter owner;
        [SerializeField] private Text uiText;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            owner.Stamina.onAttributeChanged += OnStaminaChanged;
        }

        /* ----------------------- Attribute Change Callbacks ----------------------- */

        private void OnStaminaChanged(float oldValue, float newValue)
        {
            var stamina = owner.Stamina;
            var maxStamina = owner.MaxStamina;
            uiText.text = Mathf.CeilToInt(stamina.Value / maxStamina.Value * 100f).ToString() + "%";
        }

        /* -------------------------------------------------------------------------- */
    }
}