using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    public class AIBehaviourAttack : AIBehaviour
    {

        private float turnSpeed = 2.75f;

        public override void BehaviourStart()
        {
            owner.SetMovementControl(AICharacter.AIMovementControlType.Animator);
            owner.Attack(owner.nextAttack);
        }

        public override void BehaviourUpdate()
        {
            LookAtTarget();
        }

        public override void BehaviourEnd()
        {

        }

        private void LookAtTarget()
        {
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.LookRotation(owner.GetDirectionToTarget(), Vector3.up), turnSpeed * Time.deltaTime);
        }
    }
}
