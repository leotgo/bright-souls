using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls
{
    public class LockOnDetector : MonoBehaviour
    {
        public List<Character> PossibleTargets
        {
            get
            {
                List<Character> targets = new List<Character>(possibleTargets);
                targets.RemoveAll((Character c) => { return c.IsInAnyState(States.Dead) || !c.GetComponentInChildren<Renderer>().isVisible; });
                return targets;
            }
        }

        private bool initialized = false;
        private Player owner;
        private List<Character> possibleTargets;

        private void Start()
        {
            owner = GetComponentInParent<Player>();
            possibleTargets = new List<Character>();
            initialized = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!initialized)
                return;

            Character character = other.GetComponent<Character>();
            if (!character)
                return;
            else if (character == owner)
                return;

            if (!possibleTargets.Contains(character) && !character.IsInAnyState(States.Dead))
                possibleTargets.Add(character);
        }

        private void OnTriggerExit(Collider other)
        {
            Character character = other.GetComponent<Character>();
            if (!character)
                return;
            else if (character == owner)
                return;

            if (possibleTargets.Contains(character))
                possibleTargets.Remove(character);
        }

        public void RefreshTargets()
        {
            possibleTargets.RemoveAll((Character c) => { return c.IsInAnyState(States.Dead); });
        }
    }
}