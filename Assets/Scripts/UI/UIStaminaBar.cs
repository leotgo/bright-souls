using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using BrightSouls.Player;
using DuloGames.UI;

namespace BrightSouls.UI
{
    public class UIStaminaBar : MonoBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private ICombatCharacter owner;
        [SerializeField] private UIProgressBar staminaBar;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start ()
        {
            owner.Stamina.onAttributeChanged += OnStaminaChanged;
        }

        /* ----------------------- Attribute Change Callbacks ----------------------- */

        private void OnStaminaChanged(float oldValue, float newValue)
        {
            staminaBar.fillAmount = owner.Stamina.Value / owner.MaxStamina.Value;
        }

        /* -------------------------------------------------------------------------- */
    }
}