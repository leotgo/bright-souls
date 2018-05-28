using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using Helpers.Routine;

public class CombatController : MonoBehaviour, IHitter, IHittable
{

    private Player player;

    private Character lockOnTarget;
    public Character LockOnTarget {
        get {
            return lockOnTarget;
        }
        set {
            lockOnTarget = value;
            this.Notify(Message.Combat_LockOnTarget, lockOnTarget);
            player.lockCamera.enabled = lockOnTarget != null;
            player.TPCamera.enabled = lockOnTarget == null;
        }
    }
    [HideInInspector] public float dodgeStaminaCost = 20f;
    private float blockBreakDamageModifier = 0.2f;
    private LockOnDetector detector;

    private void Start()
    {
        this.player = GetComponent<Player>();
        detector = player.GetComponentInChildren<LockOnDetector>();
    }

    private void Update()
    {
        if (!player.IsInAnyState(States.Dodging))
            RotationUpdate();

        if (LockOnTarget != null)
            if (LockOnTarget.IsInAnyState(States.Dead))
                LockOnTarget = null;

        if (Input.GetButtonDown("Jump"))
            Dodge();
        if (Input.GetButtonDown("LightAttack"))
            Attack(0);
        if (Input.GetButtonDown("HeavyAttack"))
            Attack(1);
        if (Input.GetButtonDown("LockOn"))
        {
            if (!LockOnTarget)
                LockOnTarget = GetLockOnTarget(0f);
            else
                LockOnTarget = null;
        }

        Block(Input.GetButton("Block"));
    }

    public void OnGetHit(Attack attack)
    {
        if (attack is MeleeAttack)
        {
            var meleeAtk = (MeleeAttack)attack;
            float damage = (float)Random.Range(meleeAtk.minDamage, meleeAtk.maxDamage);
            if (!player.HasStatus(Character.Status.IFrames))
            {
                if (HasBlockedAttack(attack))
                {
                    Debug.Log("Player blocked attack " + attack.Name + " from " + attack.Source.name);
                    player.Stamina.Value -= (float)meleeAtk.blockStaminaDamage;
                    if (player.Stamina.Value <= 0f)
                    {
                        this.Notify(Message.Combat_BlockBreak);
                        player.Health -= damage * blockBreakDamageModifier;
                        player.Stagger.StaggerHealth -= player.Stagger.maxStaggerHealth;
                    }
                    else
                    {
                        this.Notify(Message.Combat_BlockedHit);
                        player.anim.SetTrigger("block_hit");
                    }
                }
                else
                {
                    if (!player.IsInAnyState(States.Dead))
                    {
                        player.Health -= damage;
                        player.Stagger.StaggerHealth -= meleeAtk.staggerDamage;
                        player.Notify(Message.Combat_GotHit);
                    }
                }

            }

        }
    }

    public bool HasBlockedAttack(Attack attack)
    {
        if (player.IsInAnyState(States.Blocking))
        {
            Character enemy = attack.Source;
            Vector3 playerDir = player.GetDirectionPlanified();
            Vector3 dirToEnemy = (enemy.transform.position - player.transform.position).normalized;
            float angle = Vector3.Angle(playerDir, dirToEnemy);
            return angle < 100f ? true : false;
        }
        else
            return false;
    }

    // Will Depend on Camera Type
    private void RotationUpdate()
    {
        Vector3 fwd = (!LockOnTarget) ? Camera.main.transform.forward : (LockOnTarget.transform.position - transform.position).normalized;
        fwd.Set(fwd.x, 0f, fwd.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(fwd, Vector3.up), 15f * Time.deltaTime);
    }

    private void Dodge()
    {
        if (player.IsInAnyState(States.Attacking, States.Dodging, States.Falling, States.Jumping, States.Landing, States.Comboing, States.ComboEnding, States.Blocking, States.Stagger, States.Dead))
            return;

        if (player.Stamina.Value <= 0f)
            return;

        Vector2 dodgeDir = GetDodgeDirection();
        Vector3 camFwd = UnityEngine.Camera.main.transform.forward;
        camFwd = (new Vector3(camFwd.x, 0f, camFwd.z)).normalized;
        if (dodgeDir.y < 0.1f)
        {
            dodgeDir.Set(dodgeDir.x * -1f, dodgeDir.y);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f) * Quaternion.LookRotation(camFwd, Vector3.up);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(camFwd, Vector3.up);
        }

        player.anim.SetFloat("dodge_x", dodgeDir.x);
        player.anim.SetFloat("dodge_y", dodgeDir.y);
        player.anim.SetTrigger("dodge");
    }

    private void Attack(int id)
    {
        if (!player.IsInAnyState(States.Dodging, States.Attacking, States.ComboEnding, States.Jumping, States.Blocking, States.Stagger, States.Dead))
        {
            var weapon = GetComponentInChildren<Weapon>();
            weapon.OnAttack(id);
        }
    }

    private void Block(bool block)
    {
        if (player.IsInAnyState(States.Attacking, States.Dodging, States.Falling, States.Jumping, States.Landing, States.Comboing, States.ComboEnding, States.Stagger, States.Dead))
            return;
        player.anim.SetBool("block", block);
    }

    private static Vector2 GetDodgeDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 dodgeDir = new Vector2(0f, 0f);
        dodgeDir.Set(horizontal, vertical);

        dodgeDir = dodgeDir.magnitude > 0.2f ? dodgeDir.normalized : Vector2.zero;
        return dodgeDir;
    }

    public Character GetLockOnTarget(float dir)
    {
        Character closestInDir = LockOnTarget;
        float diffToCenter = 1.0f;
        foreach (var target in detector.possibleTargets)
        {
            if (target != LockOnTarget)
            {
                var viewPos = Camera.main.WorldToViewportPoint(target.transform.position);
                float diff = Mathf.Abs(viewPos.x - 0.5f);
                if (diff < diffToCenter)
                {
                    bool possibleTarget = false;
                    if ((dir == 0f) ||
                        (dir > 0f && viewPos.x > 0.5f) ||
                        (dir < 0f && viewPos.x < 0.5f))
                        possibleTarget = true;
                    closestInDir = possibleTarget ? target : closestInDir;
                    diffToCenter = possibleTarget ? diff : diffToCenter;
                }
            }

        }
        return closestInDir;
    }

}
