using UnityEngine;

namespace BrightSouls
{
    [CreateAssetMenu(fileName = "PlayerCombatData", menuName = "BrightSouls/Data/Player/PlayerCombatData", order = 0)]
    public sealed class PlayerCombatData : ScriptableObject
    {
        public float DodgeStaminaCost { get => dodgeStaminaCost; }
        public float BlockBreakDamageModifier { get => blockBreakDamageModifier; }
        public float MaximumBlockAngle { get => maximumBlockAngle; }
        public float LockOnBodyRotationSpeed { get => lockOnbodyRotationLerpSpeed; }

        [SerializeField] private float dodgeStaminaCost = 20f;
        [SerializeField] private float blockBreakDamageModifier = .2f;
        [SerializeField] private float maximumBlockAngle = 100f;
        [SerializeField] private float lockOnbodyRotationLerpSpeed = 7.5f;
    }
}