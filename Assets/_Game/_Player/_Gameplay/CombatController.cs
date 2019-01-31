using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using Helpers.Timing;

public class CombatController : MonoBehaviour, IHitter, IHittable
{

    private Player player;

    public float dodgeStaminaCost = 20f;
    private float blockBreakDamageModifier = 0.2f;

    public AttackCommand attack;
    public BlockCommand block;
    public DodgeCommand dodge;

    private void Start()
    {
        this.player = GetComponent<Player>();

        attack = new AttackCommand(this.player);
        block = new BlockCommand(this.player);
        dodge = new DodgeCommand(this.player);
    }

    private void Update()
    {
        if (!player.IsInAnyState(States.Dodging))
            RotationUpdate();
    }

    public void OnGetHit(Attack attack)
    {
        if (attack is MeleeAttack)
        {
            var meleeAtk = (MeleeAttack)attack;
            float damage = (float)Random.Range(meleeAtk.minDamage, meleeAtk.maxDamage);
            if (!player.HasStatus(Character.Status.IFrames))
            {
                var sourceDir = (attack.Source.transform.position - player.transform.position).normalized;
                sourceDir = Quaternion.Inverse(player.transform.rotation) * sourceDir;
                player.anim.SetFloat("damage_dir_x", sourceDir.x);
                player.anim.SetFloat("damage_dir_y", sourceDir.z);
                Debug.LogFormat("COMBAT: {0} got hit by {1} from {2}", player, attack.Source, sourceDir);
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
        if(player.Camera.Mode == PlayerCamera.CameraMode.LockOn)
        {
            var dir = player.Camera.LockOnTarget.transform.position - player.transform.position;
            dir = Vector3.ProjectOnPlane(dir, Vector3.up);
            dir = dir.normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), 7.5f * Time.deltaTime);
        }
    }

    public class AttackCommand : PlayerCommand<int>
    {
        public AttackCommand(Player owner) : base(owner) { }

        public override bool IsValid()
        {
            return !player.IsInAnyState(States.Dodging, States.Jumping, States.Blocking, States.Stagger, States.Dead)
                && player.Stamina.Value > 0f;
        }

        public override void Execute(int attackId)
        {
            player.Physics.MotionSource = PlayerPhysics.MotionSourceType.Animation;
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
            Vector3 dodgeDir = GetDodgeDirection();
            if (dodgeDir.magnitude < 0.2f)
            {
                if (player.Camera.Mode == PlayerCamera.CameraMode.ThirdPerson)
                    dodgeDir = Quaternion.Inverse(Camera.main.transform.rotation) * new Vector3(player.transform.forward.x, 0f, player.transform.forward.z).normalized;
                else
                    dodgeDir = Vector3.back;
            }
            Vector3 camFwd = UnityEngine.Camera.main.transform.forward;
            camFwd = (new Vector3(camFwd.x, 0f, camFwd.z)).normalized;
            if (player.Camera.Mode == PlayerCamera.CameraMode.ThirdPerson)
            {
                var dir = dodgeDir.magnitude > 0f ? dodgeDir : Vector3.back;
                var planifiedDir = Vector3.ProjectOnPlane(Camera.main.transform.rotation * dir, Vector3.up).normalized;
                player.transform.rotation = Quaternion.LookRotation(planifiedDir, Vector3.up);
                player.anim.SetFloat("dodge_x", 0f);
                player.anim.SetFloat("dodge_y", 1f);
            }
            else
            {
                if (dodgeDir.z < 0.1f)
                {
                    dodgeDir.Set(dodgeDir.x * -1f, 0f, dodgeDir.z);
                    player.transform.rotation = Quaternion.LookRotation(-1f * camFwd, Vector3.up);
                }
                else
                    player.transform.rotation = Quaternion.LookRotation(camFwd, Vector3.up);

                player.anim.SetFloat("dodge_x", dodgeDir.x);
                player.anim.SetFloat("dodge_y", dodgeDir.z);
            }
            player.Physics.MotionSource = PlayerPhysics.MotionSourceType.Animation;
            player.anim.SetTrigger("dodge");
        }
    }

    private static Vector3 GetDodgeDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 dodgeDir = new Vector3(0f, 0f, 0f);
        dodgeDir.Set(horizontal, 0f, vertical);

        dodgeDir = dodgeDir.magnitude > 0.2f ? dodgeDir.normalized : dodgeDir;
        return dodgeDir;
    }
}
