using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Patterns.Observer;
using Helpers.Timing;

namespace BrightSouls.AI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class AICharacter : Character, IHitter, IHittable
    {
        public enum AIMovementControlType
        {
            NavAgent,
            Behavior,
            Animator
        }

        public NavMeshAgent NavAgent
        {
            get => navAgent;
        }

        public Animator AIAnimator
        {
            get => animator;
        }

        public float CurrentMoveSpeed
        {
            get => _currentMoveSpeed;
            set => _currentMoveSpeed = value;
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

            get => _movement;
            set => _movement = value;
        }

        public Character Target
        {
            get => _target;
            set => _target = value;
        }

        public bool HasTarget
        {
            get => Target != null;
        }

        public override AttributesContainer Attributes
        {
            get => attributes;
        }

        // Events
        public delegate void OnStartAttackHandler();
        public event OnStartAttackHandler onStartAttack;

        // Component Refs
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private Animator animator;

        // Inspector-assigned values
        [SerializeField] private float walkMoveSpeed = 2f;
        [SerializeField] private float runMoveSpeed = 4f;

        // Runtime
        public int nextAttack = 0;
        private AttributesContainer attributes = new AttributesContainer();
        private Character _target = null;
        private AIMovementControlType _movementType = AIMovementControlType.NavAgent;
        private float _currentMoveSpeed = 4f;
        private Vector2 _movement; // World-Space Movement Direction (X,Z) of AICharacter

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

        public void OnGetHit(Attack attack)
        {
            if (IsInAnyState(States.Dead))
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

        public void ResetAllTriggers()
        {
            animator.ResetTrigger("hit");
            animator.ResetTrigger("stagger");
            animator.ResetTrigger("death");
            animator.ResetTrigger("attack_light");
            animator.ResetTrigger("attack_heavy");
            animator.ResetTrigger("attack_dash");
        }

        private void Update()
        {
            UpdateMove();
            UpdateAcceleration();
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
    }
}