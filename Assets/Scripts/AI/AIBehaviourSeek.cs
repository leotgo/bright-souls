using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BrightSouls.AI
{

    public class AIBehaviourSeek : AIBehaviour
    {

        [Range(0.1f, 3f)]
        public float targetPosUpdateInterval = 1f;
        private float updateTimer = 0f;

        public override void BehaviourStart()
        {
            owner.ResetAllTriggers();
            updateTimer = targetPosUpdateInterval;
            owner.navAgent.isStopped = false;
            owner.CurrentMoveSpeed = owner.runMoveSpeed;
            owner.SetMovementControl(AICharacter.AIMovementControlType.NavAgent);
        }

        public override void BehaviourUpdate()
        {
            updateTimer += Time.deltaTime;
            if (updateTimer > targetPosUpdateInterval)
            {
                updateTimer = 0f;
                owner.navAgent.SetDestination(owner.Target.transform.position);
            }
        }

        public override void BehaviourEnd()
        {
            owner.navAgent.isStopped = true;
        }
    }
}
