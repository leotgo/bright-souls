using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BrightSouls.AI
{
    public class AIBehaviourSeek : AIBehaviour
    {
        [Range(0.1f, 3f)] [SerializeField] private float targetPosUpdateInterval = 1f;

        private float updateTimer = 0f;

        public override void OnBehaviourStart(AICharacter agent)
        {
            agent.ResetAllTriggers();
            updateTimer = targetPosUpdateInterval;
            agent.NavAgent.isStopped = false;
            agent.CurrentMoveSpeed = agent.RunMoveSpeed;
            agent.SetMovementControl(AICharacter.AIMovementControlType.NavAgent);
        }

        public override void OnBehaviourUpdate(AICharacter agent)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer > targetPosUpdateInterval)
            {
                updateTimer = 0f;
                agent.NavAgent.SetDestination(agent.Target.transform.position);
            }
        }

        public override void OnBehaviourEnd(AICharacter agent)
        {
            agent.NavAgent.isStopped = true;
        }
    }
}
