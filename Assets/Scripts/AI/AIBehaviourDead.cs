using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    public class AIBehaviourDead : AIBehaviour
    {
        public override void OnBehaviourStart(AICharacter agent)
        {
            agent.enabled = false;
            DisableAllColliders(agent.gameObject);
        }

        public override void OnBehaviourUpdate(AICharacter agent)
        {
            // Do nothing
        }

        public override void OnBehaviourEnd(AICharacter agent)
        {
            // Do nothing
        }

        private void DisableAllColliders(GameObject go)
        {
            var allColliders = go.GetComponentsInChildren<Collider>();
            foreach (var collider in allColliders)
            {
                collider.enabled = false;
            }
        }
    }
}