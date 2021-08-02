using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls
{
    [RequireComponent(typeof(Collider))]
    public class GroundDetector : MonoBehaviour
    {
        public LayerMask groundLayers;
        public Vector3 groundPoint;

        private int count = 0;

        private void Start()
        {
            count = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            count++;
            groundPoint = other.ClosestPointOnBounds(transform.position);
        }

        private void OnTriggerExit(Collider other)
        {
            count--;
        }

        public bool IsGrounded {
            get {
                return count > 0;
            }
        }
    }
}