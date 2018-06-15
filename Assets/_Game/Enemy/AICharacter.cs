using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Patterns.Observer;
using Helpers.Routine;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AICharacter : Character, IHitter, IHittable
{
    // Definitions

    public enum MovementControlType
    {
        NavAgent,
        Behaviour,
        Animator
    }


    // Public fields

    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public Animator animator;

    public int nextAttack = 0;

    public float walkMoveSpeed = 2f;
    public float runMoveSpeed = 4f;

    // Private fields

    private Character _target = null;
    private MovementControlType _movementType = MovementControlType.NavAgent;
    private float _currentMoveSpeed = 4f;


    // Getters and Setters

    public float CurrentMoveSpeed {
        get {
            return _currentMoveSpeed;
        }
        set {
            _currentMoveSpeed = value;
        }
    }

    private Vector2 _movement;
    public Vector2 Movement {
        // Movement Direction ([0,1], [0,1]) of AICharacter 
        // Does not consider rotation
        get {
            return _movement;
        }
        set {
            _movement = value;
        }
    }

    public Character Target {
        get {
            return _target;
        }
        set {
            _target = value;
        }
    }

    public bool HasTarget {
        get {
            return Target != null;
        }
    }

    public float GetDistanceToTarget()
    {
        if (HasTarget)
            return Vector3.Distance(transform.position, Target.transform.position);
        else
            return Mathf.Infinity;
    }

    public Vector3 GetDirectionToTarget()
    {
        if (HasTarget)
            return (Target.transform.position - transform.position).normalized;
        else
            return Vector3.zero;
    }

    public Vector3 GetDirectionToTargetPlanified()
    {
        if (HasTarget)
        {
            var dir = GetDirectionToTarget();
            return (new Vector3(dir.x, 0f, dir.z)).normalized;
        }
        else
            return Vector3.zero;
    }


    // Public methods ============================================ //

    public void OnGetHit(Attack attack)
    {
        if (IsInAnyState(States.Dead))
            return;

        MeleeAttack meleeAtk;
        if (attack is MeleeAttack)
        {
            meleeAtk = (MeleeAttack)attack;
            var damage = Random.Range(meleeAtk.minDamage, meleeAtk.maxDamage);
            this.Health -= damage;
            if (Health > 0f)
            {
                GetComponent<StaggerBehaviour>().StaggerHealth -= meleeAtk.staggerDamage;
            }
            meleeAtk.Source.Notify(Message.Combat_HitEnemy, this);
            this.Notify(Message.Combat_GotHit);
            Time.timeScale = 0.2f;
            ActionHelper.Delay(0.06f, () => { Time.timeScale = 1f; });
        }
    }

    public void SetMovementControl(MovementControlType moveType)
    {
        this._movementType = moveType;

        if (_movementType == MovementControlType.NavAgent)
            navAgent.isStopped = false;
        else
            navAgent.isStopped = true;

        if (_movementType == MovementControlType.Animator)
            animator.applyRootMotion = true;
        else
            animator.applyRootMotion = false;
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


    // Private Methods =========================================== //

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateMove();

        animator.SetFloat("move_x", Mathf.Lerp(animator.GetFloat("move_x"),
            Movement.x, navAgent.acceleration * Time.deltaTime));
        animator.SetFloat("move_y", Mathf.Lerp(animator.GetFloat("move_y"),
            Movement.y, navAgent.acceleration * Time.deltaTime));
    }

    private void UpdateMove()
    {
        navAgent.speed = Mathf.Lerp(navAgent.speed, CurrentMoveSpeed, navAgent.acceleration * Time.deltaTime);
        if (_movementType == MovementControlType.NavAgent)
        {
            Movement =
                new Vector2(
                    Vector3.Dot(navAgent.velocity.normalized, transform.right),
                    Vector3.Dot(navAgent.velocity.normalized, transform.forward)
                    ).normalized;
            Movement *= (navAgent.velocity.magnitude / runMoveSpeed);
        }
        else if (_movementType == MovementControlType.Behaviour)
        {
            var moveVec = 
                transform.rotation 
                * new Vector3(Movement.x, 0f, Movement.y).normalized 
                * navAgent.speed;
            navAgent.Move(moveVec * Time.deltaTime);
        }
    }

}
