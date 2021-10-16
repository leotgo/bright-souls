using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;
using BrightSouls.Gameplay;

namespace BrightSouls.UI
{
    public class UIControllerEnemy : MonoBehaviour
    {
        /* ------------------------------- Properties ------------------------------- */

        public PlayerCombatController PlayerCombat
        {
            get => playerCombat;
            set
            {
                if(playerCombat == null)
                {
                    playerCombat = value;
                    playerCombat.onLockedOn += OnPlayerLockOn;
                }
                else
                {
                    playerCombat = value;
                }
            }
        }

        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private Canvas UIElementCanvas;
        [SerializeField] private ICombatCharacter owner;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private TimerAction disableUITimer;
        private PlayerCombatController playerCombat;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            UIElementCanvas.enabled = false;
            disableUITimer = new TimerAction(this, 5f, () => UIElementCanvas.enabled = false);
            owner.Health.onAttributeChanged += OnHealthChanged;
        }

        /* ----------------------------- Event Callbacks ---------------------------- */

        private void OnHealthChanged(float oldValue, float newValue)
        {
            if (owner.IsDead)
            {
                UIElementCanvas.enabled = false;
            }
            if (!UIElementCanvas.enabled)
            {
                UIElementCanvas.enabled = true;
                disableUITimer.Start();
            }
        }

        private void OnPlayerLockOn(ICombatCharacter playerTarget)
        {
            if (playerTarget == owner)
            {
                UIElementCanvas.enabled = true;
                disableUITimer.Stop();
            }
            else if (!disableUITimer.IsCounting())
            {
                UIElementCanvas.enabled = false;
            }
        }

        /* -------------------------------------------------------------------------- */
    }
}