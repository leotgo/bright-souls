using UnityEngine;
using Patterns.Observer;

namespace BrightSouls
{
    public sealed class PlayerCombatController : MonoBehaviour, IHitter, IHittable
    {
        /* ------------------------------- Properties ------------------------------- */

        public PlayerCombatData Data { get => data; }
        public PlayerCombatEvents Events { get => events; }
        public PlayerCombatCommands Commands { get => commands; }

        /* --------------------------------- Fields --------------------------------- */

        [SerializeField] private Player player;
        [SerializeField] private PlayerCombatData data;
        [SerializeField] private PlayerCombatEvents events;
        [SerializeField] private PlayerCombatCommands commands;

        /* ------------------------- MonoBehaviour Callbacks ------------------------ */

        private void Start()
        {
            InitializeComponentReferences();
            InitializeInput();
        }

        private void Update()
        {
            bool playerHasTarget = player.CameraDirector?.CurrentCamera?.IsLockOnCamera ?? false;
            bool playerIsDodging = player.IsInAnyState(States.Dodging);
            if (playerHasTarget && !playerIsDodging)
                FaceTarget();
        }

        /* ----------------------------- Initialization ----------------------------- */

        private void InitializeComponentReferences()
        {
            commands = new PlayerCombatCommands(player);
        }

        private void InitializeInput()
        {
            var attack = player.Input.currentActionMap.FindAction("Attack");
            var defend = player.Input.currentActionMap.FindAction("Defend");

            attack.performed += ctx => commands.Attack.Execute(0);
            defend.started += ctx => commands.Defend.Execute(true);
            defend.canceled += ctx => commands.Defend.Execute(false);
        }

        /* ---------------------------- Event Processing ---------------------------- */

        public void OnGetHit(Attack attack)
        {
            bool playerIsInvincible = player.Attributes.GetAttribute<StatusAttribute>().HasStatus(CharacterStatus.IFrames);
            if (playerIsInvincible)
            {
                return;
            }

            // Determine source dir of the attack
            var attackSourceDirection = (attack.Source.transform.position - player.transform.position).normalized;

            // TODO Separate animation handling from combat processing
            player.Anim.SetFloat("damage_dir_x", attackSourceDirection.x);
            player.Anim.SetFloat("damage_dir_y", attackSourceDirection.z);

            // TODO Separate logging from combat processing
            Debug.LogFormat("COMBAT: {0} got hit by {1} from {2}", player, attack.Source, attackSourceDirection);

            // Check blocking status
            bool playerIsBlocking = player.IsInAnyState(States.Blocking);
            bool hasSuccesfullyBlocked = CheckBlockSuccess(attack);

            // TODO separate behaviors in different functions
            // Behavior 1: blocked attack
            if (playerIsBlocking && hasSuccesfullyBlocked)
            {
                //player.Stamina.Value -= (float)attack.Data.BlockStaminaDamage;
                if (player.Stamina.Value <= 0f)
                {
                    events.RaiseOnBreakBlockEvent();
                    //player.Health -= damage * data.BlockBreakDamageModifier;
                    //player.Stagger.StaggerHealth -= player.Stagger.maxStaggerHealth;
                }
                else
                {
                    events.RaiseOnBlockHitEvent();
                }
            }
            // Behavior 2: did not block attack
            else
            {
                bool playerIsDead = player.IsInAnyState(States.Dead);
                if (!playerIsDead)
                {
                    //player.Health -= damage;
                    //player.Stagger.StaggerHealth -= attack.Data.staggerDamage;
                    events.RaiseOnTakeDamageEvent();
                }
            }
        }

        /* --------------------------------- Helpers -------------------------------- */

        /// <summary>
        /// Checks if an attack can be succesfully blocked by the player,
        /// by verifying that the direction the attack is coming from
        /// does not diverge too much from where the player is facing.
        /// </summary>
        private bool CheckBlockSuccess(Attack attack)
        {
            Character enemy = attack.Source;
            var dirPlayerForward = player.GetDirectionInXZPlane();
            var dirPlayerToEnemy = (enemy.transform.position - player.transform.position).normalized;
            float angle = Vector3.Angle(dirPlayerForward, dirPlayerToEnemy);
            return angle < data.MaximumBlockAngle;
        }

        /// <summary>
        /// Rotates the player character's body so it points towards
        /// the locked-on target.
        /// </summary>
        private void FaceTarget()
        {
            var lockOnCamera = player.CameraDirector.GetCamera<LockOnCamera>();
            var targetPosition = lockOnCamera.Target.transform.position;
            var targetOffset = targetPosition - player.transform.position;
            // Ignore vertical position difference so the player doesn't face upwards or downwards.
            targetOffset.Set(targetOffset.x, 0f, targetOffset.z);
            var directionToTarget = targetOffset.normalized;
            // Make the body face the target (with Linear Interpolation smoothing)
            var rotationToTarget = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationToTarget, data.LockOnBodyRotationSpeed * Time.deltaTime);
        }

        public Vector3 ReadDodgeDirectionInCameraSpace()
        {
            // Read input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Transform input to direction in XZ Plane
            var dodgeDir = new Vector3(0f, 0f, 0f);
            dodgeDir.Set(horizontal, 0f, vertical);

            // Normalize dodge direction - magnitude should either be 1 or 0
            dodgeDir = dodgeDir.magnitude > 0.2f ? dodgeDir.normalized : Vector3.zero;
            return dodgeDir;
        }

        /* -------------------------------------------------------------------------- */
    }
}