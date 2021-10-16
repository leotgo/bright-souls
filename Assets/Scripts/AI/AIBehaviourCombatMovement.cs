using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using BrightSouls.Gameplay;

namespace BrightSouls.AI
{
    public class AIBehaviourCombatMovement : AIBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private float minTargetDistance = 1.5f;
        [SerializeField] private float maxTargetDistance = 5f;
        [SerializeField] private float idealTargetDistance = 4f;
        [SerializeField] private float idealTargetDistanceThreshold = 0.2f;
        [SerializeField] private float turnSpeed = 4f;
        [SerializeField] private float minimalAttackCooldown = 0.5f;
        [SerializeField] private float defaultAttackCooldown = 3f;
        [SerializeField] private float defenseDelay = 1.5f;
        [SerializeField] private float dodgeChance = 0.35f;
        [SerializeField] private float dashAttackThreshold = 4f;
        [SerializeField] private int   lightAttackChance = 70;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private bool isAdjustingDistance = false;

        /* -------------------------- State Machine Events -------------------------- */

        public override void OnBehaviourStart(AICharacter agent)
        {
            isAdjustingDistance = false;
            agent.SetMovementControl(AICharacter.AIMovementControlType.Animator);
            agent.Movement = Vector2.zero;
            agent.CurrentMoveSpeed = agent.WalkMoveSpeed;
        }

        public override void OnBehaviourUpdate(AICharacter agent)
        {
            LookAtTarget(agent);
            AdjustCombatDistance(agent);
            ChooseAttack(agent);
        }

        public override void OnBehaviourEnd(AICharacter agent)
        {
        }

        /* --------------------------------- Helpers -------------------------------- */

        private void LookAtTarget(AICharacter agent)
        {
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.LookRotation(agent.GetDirectionToTarget(), Vector3.up), turnSpeed * Time.deltaTime);
        }

        private void AdjustCombatDistance(AICharacter agent)
        {
            float dist = agent.GetDistanceToTarget();
            float distanceDiff = Mathf.Abs(dist - idealTargetDistance);

            if (isAdjustingDistance)
            {
                if (distanceDiff < idealTargetDistanceThreshold)
                {
                    isAdjustingDistance = false;
                    agent.CurrentMoveSpeed = 0f;
                    agent.Movement = new Vector2(agent.Movement.x, 0f);
                }
            }
            else
            {
                if (dist > maxTargetDistance)
                {
                    isAdjustingDistance = true;
                    agent.CurrentMoveSpeed = agent.WalkMoveSpeed;
                    agent.Movement = new Vector2(agent.Movement.x, 0.5f);
                }
                else if (dist < minTargetDistance)
                {
                    isAdjustingDistance = true;
                    agent.CurrentMoveSpeed = agent.WalkMoveSpeed;
                    agent.Movement = new Vector2(agent.Movement.x, -0.5f);
                }
            }
        }

        private void ChooseAttack(AICharacter agent)
        {
            bool attackOnCooldown = agent.Fsm.CurrentStateTime < defaultAttackCooldown;
            bool comboOnCooldown = agent.Fsm.CurrentStateTime < minimalAttackCooldown;
            bool isInRange = agent.GetDistanceToTarget() < maxTargetDistance;
            bool targetIsAttacking = agent.Target.IsAttacking;
            bool canAttack = !attackOnCooldown || (targetIsAttacking && isInRange && !comboOnCooldown);
            if (canAttack)
            {
                if (agent.Target.IsBlocking)
                {
                    int rand = Random.Range(0, 100);
                    agent.nextAttack = rand < 30 ? 0 : 1;
                }
                else
                {
                    if (agent.GetDistanceToTarget() > idealTargetDistance + dashAttackThreshold)
                    {
                        agent.nextAttack = 2;
                    }
                    else
                    {
                        int rand = Random.Range(0, 100);
                        if (rand < lightAttackChance)
                        {
                            agent.nextAttack = 0;
                        }
                        else
                        {
                            agent.nextAttack = 1;
                        }
                    }
                }
                agent.Notify(Message.AI_StartAttack);
            }
        }

        /* -------------------------------------------------------------------------- */
    }
}