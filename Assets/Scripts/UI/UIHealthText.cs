using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

namespace BrightSouls.UI
{
    public class UIHealthText : MonoBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private ICombatCharacter owner;
        [SerializeField] private Text uiText;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start ()
        {
            owner.Health.onAttributeChanged += OnHealthChanged;
        }

        /* ----------------------- Attribute Change Callbacks ----------------------- */

        private void OnHealthChanged(float oldValue, float newValue)
        {
            uiText.text = Mathf.CeilToInt(owner.Health.Value / owner.MaxHealth.Value * 100f).ToString() + "%";
        }

        /* -------------------------------------------------------------------------- */
    }
}