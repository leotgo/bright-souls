using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using BrightSouls.Player;

namespace BrightSouls
{
    public class LockOnDetector : MonoBehaviour
    {
        public List<ICombatCharacter> PossibleTargets
        {
            get
            {
                List<ICombatCharacter> targets = new List<ICombatCharacter>(possibleTargets);
                // TODO change PlayerStateDead to a generic dead character state
                targets.RemoveAll((ICombatCharacter character) => { return character.IsDead || !character.transform.GetComponentInChildren<Renderer>().isVisible; });
                return targets;
            }
        }

        private bool initialized = false;
        private ICombatCharacter owner;
        private List<ICombatCharacter> possibleTargets;

        private void Start()
        {
            owner = GetComponentInParent<PlayerComponentIndex>();
            possibleTargets = new List<ICombatCharacter>();
            initialized = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!initialized)
            {
                return;
            }

            ICombatCharacter character = other.GetComponent<ICombatCharacter>();
            if (character != null)
            {
                return;
            }
            else if (character == owner)
            {
                return;
            }

            if (!possibleTargets.Contains(character) && !character.IsDead)
            {
                possibleTargets.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            ICombatCharacter character = other.GetComponent<ICombatCharacter>();
            if (character != null)
            {
                return;
            }
            else if (character == owner)
            {
                return;
            }

            if (possibleTargets.Contains(character))
                possibleTargets.Remove(character);
        }

        public void RefreshTargets()
        {
            RemoveAllDeadCharacters();
        }

        private void RemoveAllDeadCharacters()
        {
            possibleTargets.RemoveAll((ICombatCharacter character) => character.IsDead);
        }
    }
}