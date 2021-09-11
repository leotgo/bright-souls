using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using DuloGames.UI;

namespace BrightSouls.UI
{
    public class UIHealthBar : MonoBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private ICombatCharacter owner;
        [SerializeField] private UIProgressBar healthBar;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start ()
        {
            owner.Health.onAttributeChanged += OnHealthChanged;
        }

        /* ----------------------- Attribute Change Callbacks ----------------------- */

        private void OnHealthChanged(float oldValue, float newValue)
        {
            healthBar.fillAmount = owner.Health.Value / owner.MaxHealth.Value;
        }

        /* -------------------------------------------------------------------------- */
    }
}