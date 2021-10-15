using UnityEngine;

namespace BrightSouls
{
    public interface ICombatCharacter
    {
        Transform transform { get; }

        HealthAttribute Health { get; }
        StaminaAttribute Stamina { get; }
        PoiseAttribute Poise { get; }
        MaxHealthAttribute MaxHealth { get; }
        MaxStaminaAttribute MaxStamina { get; }
        MaxPoiseAttribute MaxPoise { get; }
        FactionAttribute Faction { get; }
        StatusAttribute Status { get; }

        bool IsDead { get; }
        bool IsAttacking { get; }
        bool IsStaggered { get; }
        bool IsBlocking { get; }
    }
}