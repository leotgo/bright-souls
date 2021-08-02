using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    [RequireComponent(typeof(Collider))]
    public class AIDetectionArea : MonoBehaviour
    {

        private Collider coll;
        private AICharacter owner;

        public LayerMask sightRaycastLayer;

        private void Start()
        {
            owner = GetComponentInParent<AICharacter>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 dir = (other.transform.position - transform.position).normalized;
            Ray r = new Ray(transform.position, dir);
            Character otherCharacter = other.GetComponent<Character>();
            if (otherCharacter != null)
            {
                owner.Target = otherCharacter;
            }
        }

        private void OnTriggerExit(Collider other)
        {

        }

    }
}