using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls.AI
{

    public class AIBehaviourCombatMovement : AIBehaviour
    {

        // Public Fields

        public float minTargetDistance = 1.5f;
        public float maxTargetDistance = 5f;
        public float idealTargetDistance = 4f;
        public float idealTargetDistanceThreshold = 0.2f;

        private float turnSpeed = 4f;

        [SerializeField] private float minimalAttackCooldown = 0.5f;
        [SerializeField] private float defaultAttackCooldown = 3f;
        public float defenseDelay = 1.5f;
        public float dodgeChance = 0.35f;

        private float dashAttackThreshold = 4f;

        private int lightAttackChance = 70;

        // Private Fields

        private bool isAdjustingDistance = false;

        // Public Methods

        public override void BehaviourStart()
        {
            isAdjustingDistance = false;

            owner.SetMovementControl(AICharacter.AIMovementControlType.Animator);
            owner.Movement = Vector2.zero;
            owner.CurrentMoveSpeed = owner.walkMoveSpeed;
        }

        public override void BehaviourUpdate()
        {
            LookAtTarget();
            AdjustCombatDistance();
            ChooseAttack();
        }

        public override void BehaviourEnd()
        {
        }

        // Private Methods

        private void LookAtTarget()
        {
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.LookRotation(owner.GetDirectionToTarget(), Vector3.up), turnSpeed * Time.deltaTime);
        }

        private void AdjustCombatDistance()
        {
            float dist = owner.GetDistanceToTarget();
            float distanceDiff = Mathf.Abs(dist - idealTargetDistance);

            if (isAdjustingDistance)
            {
                if (distanceDiff < idealTargetDistanceThreshold)
                {
                    isAdjustingDistance = false;
                    owner.CurrentMoveSpeed = 0f;
                    owner.Movement = new Vector2(owner.Movement.x, 0f);
                }
            }
            else
            {
                if (dist > maxTargetDistance)
                {
                    isAdjustingDistance = true;
                    owner.CurrentMoveSpeed = owner.walkMoveSpeed;
                    owner.Movement = new Vector2(owner.Movement.x, 0.5f);
                }
                else if (dist < minTargetDistance)
                {
                    isAdjustingDistance = true;
                    owner.CurrentMoveSpeed = owner.walkMoveSpeed;
                    owner.Movement = new Vector2(owner.Movement.x, -0.5f);
                }
            }
        }

        private void ChooseAttack()
        {
            bool canAttack =
                (fsm.CurrentStateTime > defaultAttackCooldown) ||
                (owner.Target.IsInAnyState(States.Attacking, States.Comboing) && owner.GetDistanceToTarget() < maxTargetDistance && fsm.CurrentStateTime > minimalAttackCooldown);
            if (canAttack)
            {
                if (owner.Target.IsInAnyState(States.Blocking))
                {
                    int rand = Random.Range(0, 100);
                    owner.nextAttack = rand < 30 ? 0 : 1;
                }
                else
                {
                    if (owner.GetDistanceToTarget() > idealTargetDistance + dashAttackThreshold)
                        owner.nextAttack = 2;
                    else
                    {
                        int rand = Random.Range(0, 100);
                        if (rand < lightAttackChance)
                            owner.nextAttack = 0;
                        else
                            owner.nextAttack = 1;
                    }
                }

                owner.Notify(Message.AI_StartAttack);
            }
        }
    }
}