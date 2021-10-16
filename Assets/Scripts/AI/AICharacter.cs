using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Patterns.Observer;
using UnityPatterns.FiniteStateMachine;
using Helpers.Timing;
using BrightSouls;
using BrightSouls.Gameplay;

namespace BrightSouls.AI
{
    // TODO Rename AICharacter class to AIAgent
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class AICharacter : MonoBehaviour, IStateMachineOwner, IHitter, IHittable
    {
        /* ------------------------------- Definitions ------------------------------ */

        public enum AIMovementControlType
        {
            NavAgent,
            Behavior,
            Animator
        }

        /* ------------------------------- Properties ------------------------------- */

        public NavMeshAgent NavAgent
        {
            get => navAgent;
        }

        public Animator AIAnimator
        {
            get => animator;
        }

        // TODO Separate movement-related fields to AIAgentMotor class
        public float CurrentMoveSpeed
        {
            get => currentMoveSpeed;
            set => currentMoveSpeed = value;
        }

        public float RunMoveSpeed
        {
            get => runMoveSpeed;
        }

        public float WalkMoveSpeed
        {
            get => walkMoveSpeed;
        }

        public Vector2 Movement
        {
            get => movement;
            set => movement = value;
        }

        public ICombatCharacter Target
        {
            get => target;
            set => target = value;
        }

        public bool HasTarget
        {
            get => Target != null;
        }

        public AttributesContainer Attributes
        {
            get => attributes;
        }

        public StateMachineController Fsm
        {
            // TODO Implement State Machine for AI Characters
            get => null;
        }

        /* --------------------------------- Events --------------------------------- */

        public delegate void OnStartAttackHandler();
        public event OnStartAttackHandler onStartAttack;

        /* -------------------------- Component References -------------------------- */

        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private Animator animator;

        /* ------------------------ Inspector-assigned Fields ----------------------- */

        [SerializeField] private float walkMoveSpeed = 2f;
        [SerializeField] private float runMoveSpeed = 4f;

        /* ----------------------------- Runtime Fields ----------------------------- */

        public int nextAttack = 0;
        private AttributesContainer attributes = new AttributesContainer();
        private ICombatCharacter target = null;
        private AIMovementControlType _movementType = AIMovementControlType.NavAgent;
        private float currentMoveSpeed = 4f;
        private Vector2 movement; // World-Space Movement Direction (X,Z) of AICharacter

        /* ------------------------------ Unity Events ------------------------------ */

        private void Update()
        {
            UpdateMove();
            UpdateAcceleration();
        }

        /* --------------------------- Core Functionality --------------------------- */

        public void OnGetHit(Attack attack)
        {
            if (this.Fsm.IsInState<PlayerStateDead>())
            {
                return;
            }

            MeleeAttack meleeAtk;
            if (attack is MeleeAttack)
            {
                meleeAtk = (MeleeAttack)attack;
                meleeAtk.Source.Notify(Message.Combat_HitEnemy, this);
                this.Notify(Message.Combat_GotHit);
                Time.timeScale = 0.2f;
                ActionHelper.Delay(0.06f, () => { Time.timeScale = 1f; });
            }
        }

        public void SetMovementControl(AIMovementControlType moveType)
        {
            this._movementType = moveType;

            if (_movementType == AIMovementControlType.NavAgent)
            {
                navAgent.isStopped = false;
            }
            else
            {
                navAgent.isStopped = true;
            }

            if (_movementType == AIMovementControlType.Animator)
            {
                animator.applyRootMotion = true;
            }
            else
            {
                animator.applyRootMotion = false;
            }
        }

        public void Attack(int attackId)
        {
            var weapon = GetComponentInChildren<Weapon>();
            weapon.OnAttack(attackId);
        }

        private void UpdateMove()
        {
            navAgent.speed = Mathf.Lerp(navAgent.speed, CurrentMoveSpeed, navAgent.acceleration * Time.deltaTime);
            if (_movementType == AIMovementControlType.NavAgent)
            {
                var normalizedNavAgentMovement = new Vector2(
                        Vector3.Dot(navAgent.velocity.normalized, transform.right),
                        Vector3.Dot(navAgent.velocity.normalized, transform.forward)
                        ).normalized;
                Movement = normalizedNavAgentMovement;
                Movement *= (navAgent.velocity.magnitude / runMoveSpeed);
            }
            else if (_movementType == AIMovementControlType.Behavior)
            {
                var normalizedMovement = new Vector3(Movement.x, 0f, Movement.y).normalized;
                var moveVec = transform.rotation * normalizedMovement * navAgent.speed;
                navAgent.Move(moveVec * Time.deltaTime);
            }
        }

        private void UpdateAcceleration()
        {
            var x = animator.GetFloat("move_x");
            x = Mathf.Lerp(x, Movement.x, navAgent.acceleration * Time.deltaTime);
            animator.SetFloat("move_x", x);

            var y = animator.GetFloat("move_y");
            y = Mathf.Lerp(y, Movement.y, navAgent.acceleration * Time.deltaTime);
            animator.SetFloat("move_y", y);
        }

        /* --------------------------------- Helpers -------------------------------- */

        public float GetDistanceToTarget()
        {
            if (HasTarget)
            {
                return Vector3.Distance(transform.position, Target.transform.position);
            }
            else
            {
                return Mathf.Infinity;
            }
        }

        public Vector3 GetDirectionToTarget()
        {
            if (HasTarget)
            {
                return (Target.transform.position - transform.position).normalized;
            }
            else
            {
                return Vector3.zero;
            }
        }

        public Vector3 GetDirectionToTargetPlanified()
        {
            if (HasTarget)
            {
                var dir = GetDirectionToTarget();
                return (new Vector3(dir.x, 0f, dir.z)).normalized;
            }
            else
            {
                return Vector3.zero;
            }
        }

        // TODO Call this on all AI State Transitions
        public void ResetAllTriggers()
        {
            animator.ResetTrigger("hit");
            animator.ResetTrigger("stagger");
            animator.ResetTrigger("death");
            animator.ResetTrigger("attack_light");
            animator.ResetTrigger("attack_heavy");
            animator.ResetTrigger("attack_dash");
        }

        /* -------------------------------------------------------------------------- */
    }
}