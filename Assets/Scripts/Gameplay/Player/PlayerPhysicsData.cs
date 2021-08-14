using UnityEngine;

namespace BrightSouls
{
    public class PlayerPhysicsData : ScriptableObject
    {
        public LayerMask GroundDetectionLayers { get => groundDetectionLayers; }
        public float AccelerationTime { get => accelerationTime; }
        public float DeccelerationTime { get => DeccelerationTime; }
        public float MinimumFallDamageSpeed { get => minimumFallDamageSpeed; }
        public float FallDamageMultiplier { get => fallDamageMultiplier; }
        public float BlockingMoveSpeedMultiplier { get => blockingMoveSpeedMultiplier; }

        [SerializeField] private LayerMask groundDetectionLayers;
        [SerializeField] private float accelerationTime = 1f;
        [SerializeField] private float deccelerationTime = 1f;
        [SerializeField] private float minimumFallDamageSpeed = 15f;
        [SerializeField] private float fallDamageMultiplier = 3f;
        [SerializeField] private float blockingMoveSpeedMultiplier = 0.5f;
    }
}