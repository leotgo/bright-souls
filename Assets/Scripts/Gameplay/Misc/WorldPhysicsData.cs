using UnityEngine;

namespace BrightSouls
{
    public class WorldPhysicsData : ScriptableObject
    {
        public Vector3 Gravity { get => gravity; }

        [SerializeField] private Vector3 gravity = new Vector3(0f, -9.81f, 0f);
    }
}