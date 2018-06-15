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

    public AttackCommand attack;
    public BlockCommand block;
    public DodgeCommand dodge;
    public LockOnCommand lockOn;
    public LockOnChangeCommand lockOnChange;
    private float lockOnChangeMx = 0.5f;
    private float accumMx = 0f;

    private void Start()
    {
        this.player = GetComponent<Player>();
        detector = player.GetComponentInChildren<LockOnDetector>();

        attack = new AttackCommand(this.player);
        block = new BlockCommand(this.player);
        dodge = new DodgeCommand(this.player);
        lockOn = new LockOnCommand(this.player);
        lockOnChange = new LockOnChangeCommand(this.player);
    }

    private void Update()
    {
        accumMx += Input.GetAxisRaw("Mouse X") * Time.deltaTime;
        if (Mathf.Abs(accumMx) > lockOnChangeMx && LockOnTarget != null)
        {
            accumMx = 0f;
            if (lockOnChange.IsValid())
                lockOnChange.Execute(accumMx);
        }
        accumMx = Mathf.Lerp(accumMx, 0f, Time.deltaTime);

        if (!player.IsInAnyState(States.Dodging))
            RotationUpdate();

        if (LockOnTarget != null)
            if (LockOnTarget.IsInAnyState(States.Dead))
                LockOnTarget = null;

        if (Input.GetButtonDown("Dodge"))
            if (dodge.IsValid())
                dodge.Execute();
        if (Input.GetButtonDown("LightAttack"))
            if (attack.IsValid())
                attack.Execute(0);
        if (Input.GetButtonDown("HeavyAttack"))
            if (attack.IsValid())
                attack.Execute(1);
        if (Input.GetButtonDown("LockOn"))
            if (lockOn.IsValid())
                lockOn.Execute();



        if(block.IsValid())
            block.Execute(Input.GetButton("Block"));
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

    private void RotationUpdate()
    {
        Vector3 fwd = (!LockOnTarget) ? Camera.main.transform.forward : (LockOnTarget.transform.position - transform.position).normalized;
        fwd.Set(fwd.x, 0f, fwd.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(fwd, Vector3.up), 15f * Time.deltaTime);
    }

    public class AttackCommand : PlayerCommand<int>
    {
        public AttackCommand(Player owner) : base(owner) { }

        public override bool IsValid()
        {
            return !player.IsInAnyState(States.Dodging, States.Attacking, States.ComboEnding, States.Jumping, States.Blocking, States.Stagger, States.Dead)
                && player.Stamina.Value > 0f;
        }

        public override void Execute(int attackId)
        { 
            var weapon = player.GetComponentInChildren<Weapon>();
            weapon.OnAttack(attackId);
        }
    }

    public class BlockCommand : PlayerCommand<bool>
    {
        public BlockCommand(Player owner) : base(owner) { }

        public override bool IsValid()
        {
            return !player.IsInAnyState(States.Attacking, States.Dodging, States.Falling, States.Jumping, States.Landing, States.Comboing, States.ComboEnding, States.Stagger, States.Dead);
        }

        public override void Execute(bool block)
        {
            player.anim.SetBool("block", block);
        }
    }

    public class DodgeCommand : PlayerCommand
    {
        public DodgeCommand(Player player) : base(player) { }

        public override bool IsValid()
        {
            return !player.IsInAnyState(States.Attacking, States.Dodging, States.Falling, States.Jumping, States.Landing, States.Comboing, States.ComboEnding, States.Blocking, States.Stagger, States.Dead)
                && player.Stamina.Value > 0f;
        }

        public override void Execute()
        {
            Vector2 dodgeDir = GetDodgeDirection();
            Vector3 camFwd = UnityEngine.Camera.main.transform.forward;
            camFwd = (new Vector3(camFwd.x, 0f, camFwd.z)).normalized;
            if (dodgeDir.y < 0.1f)
            {
                dodgeDir.Set(dodgeDir.x * -1f, dodgeDir.y);
                player.transform.rotation = Quaternion.Euler(0f, 180f, 0f) * Quaternion.LookRotation(camFwd, Vector3.up);
            }
            else
            {
                player.transform.rotation = Quaternion.LookRotation(camFwd, Vector3.up);
            }

            player.anim.SetFloat("dodge_x", dodgeDir.x);
            player.anim.SetFloat("dodge_y", dodgeDir.y);
            player.anim.SetTrigger("dodge");
        }
    }

    public class LockOnCommand : PlayerCommand
    {
        public LockOnCommand(Player player) : base(player) { }

        public override bool IsValid()
        {
            return player.Combat.detector.PossibleTargets.Count > 0;
        }

        public override void Execute()
        {
            if (!player.Combat.LockOnTarget)
                player.Combat.LockOnTarget = player.Combat.GetLockOnTarget(0f);
            else
                player.Combat.LockOnTarget = null;
        }
    }

    public class LockOnChangeCommand : PlayerCommand<float>
    {
        public LockOnChangeCommand(Player player) : base (player) { }

        public override bool IsValid()
        {
            return player.Combat.detector.PossibleTargets.Count > 1;
        }

        public override void Execute(float dir)
        {
            player.Combat.LockOnTarget = player.Combat.GetLockOnTarget(dir);
        }
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
        detector.RefreshTargets();
        foreach (var target in detector.PossibleTargets)
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
