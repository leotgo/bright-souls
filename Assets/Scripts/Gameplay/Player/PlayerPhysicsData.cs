using UnityEngine;

namespace BrightSouls
{
    [CreateAssetMenu(fileName = "PlayerPhysicsData", menuName = "BrightSouls/Physics/PlayerPhysicsData", order = 1)]
    public class PlayerPhysicsData : ScriptableObject
    {
        /* ------------------------------- Properties ------------------------------- */

        public LayerMask GroundDetectionLayers
        {
            get => groundDetectionLayers;
        }

        public float AccelerationTime
        {
            get => accelerationTime;
        }

        public float DeccelerationTime
        {
            get => DeccelerationTime;
        }

        public float MinimumFallDamageSpeed
        {
            get => minimumFallDamageSpeed;
        }

        public float FallDamageMultiplier
        {
            get => fallDamageMultiplier;
        }

        public float BlockingMoveSpeedMultiplier
        {
            get => blockingMoveSpeedMultiplier;
        }

        /* ----------------------------- Serialized Data ---------------------------- */

        // Ground detection
        [SerializeField] private LayerMask groundDetectionLayers;

        // Movement physics
        [SerializeField] private float accelerationTime = 1f;
        [SerializeField] private float deccelerationTime = 1f;

        // Fall damage
        [SerializeField] private float minimumFallDamageSpeed = 15f;
        [SerializeField] private float fallDamageMultiplier = 3f;

        // Movement speed modifiers
        [SerializeField] private float blockingMoveSpeedMultiplier = 0.5f;

        /* -------------------------------------------------------------------------- */
    }
}