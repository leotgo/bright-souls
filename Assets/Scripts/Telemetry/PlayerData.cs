using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BrightSouls.Telemetry
{
    [Serializable]
    public class PlayerData : SaveData
    {
        [Serializable]
        private struct Vector3Data
        {
            public float x;
            public float y;
            public float z;

            public Vector3Data(Vector3 v)
            {
                x = v.x;
                y = v.y;
                z = v.z;
            }
        };

        [Serializable]
        private struct QuaternionData
        {
            public float x;
            public float y;
            public float z;
            public float w;

            public QuaternionData(Quaternion q)
            {
                x = q.x;
                y = q.y;
                z = q.z;
                w = q.w;
            }
        }

        [NonSerialized] public Player player;

        public float health;
        public float stamina;
        private Vector3Data position;
        public Vector3 Position
        {
            get
            {
                return new Vector3(position.x, position.y, position.z);
            }
        }

        private QuaternionData rotation;
        public Quaternion Rotation
        {
            get
            {
                return new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            }
        }

        public void SaveAll()
        {
            //health = player.Health;
            //stamina = player.Stamina.Value;
            position = new Vector3Data(player.transform.position);
            rotation = new QuaternionData(player.transform.rotation);
        }
    }
}