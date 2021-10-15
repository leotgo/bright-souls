using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{
    [System.Serializable]
    public class AIBehaviourAttack : AIBehaviour
    {
        /* ----------------------- Inspector-Assigned Values; ----------------------- */

        [SerializeField] private float turnSpeed = 2.75f;

        /* -------------------------- State Machine Events -------------------------- */

        public override void OnBehaviourStart(AICharacter agent)
        {
            agent.SetMovementControl(AICharacter.AIMovementControlType.Animator);
            agent.Attack(agent.nextAttack);
        }

        public override void OnBehaviourUpdate(AICharacter agent)
        {
            LookAtTarget(agent);
        }

        public override void OnBehaviourEnd(AICharacter agent)
        {

        }

        /* --------------------------------- Helpers -------------------------------- */

        private void LookAtTarget(AICharacter agent)
        {
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.LookRotation(agent.GetDirectionToTarget(), Vector3.up), turnSpeed * Time.deltaTime);
        }

        /* -------------------------------------------------------------------------- */
    }
}
