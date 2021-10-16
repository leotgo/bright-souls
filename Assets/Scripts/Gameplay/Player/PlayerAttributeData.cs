using UnityEngine;

namespace BrightSouls.Gameplay
{
    public sealed class PlayerAttributeData : ScriptableObject
    {
        /* ------------------------------- Properties ------------------------------- */

        public float Health
        {
            get => health;
        }

        public float MaxHealth
        {
            get => maxHealth;
        }

        public float Stamina
        {
            get => stamina;
        }

        public float MaxStamina
        {
            get => maxStamina;
        }

        public float Poise
        {
            get => poise;
        }

        public float MaxPoise
        {
            get => maxPoise;
        }

        /* ------------------------ Inspector-assigned Fields ----------------------- */

        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private float stamina;
        [SerializeField] private float maxStamina;
        [SerializeField] private float poise;
        [SerializeField] private float maxPoise;

        /* -------------------------------------------------------------------------- */
    }
}