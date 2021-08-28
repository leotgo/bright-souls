using UnityEngine;

namespace BrightSouls
{
    [CreateAssetMenu(fileName = "WorldPhysicsData", menuName = "BrightSouls/Physics/WorldPhysicsData", order = 1)]
    public class WorldPhysicsData : ScriptableObject
    {
        /* ------------------------------- Properties ------------------------------- */

        public Vector3 Gravity
        {
            get => gravity;
        }

        /* ----------------------------- Serialized Data ---------------------------- */

        [SerializeField] private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

        /* -------------------------------------------------------------------------- */
    }
}