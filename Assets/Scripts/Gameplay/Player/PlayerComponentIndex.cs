using UnityEngine;
using UnityEngine.InputSystem;

namespace BrightSouls.Player
{
    public sealed class PlayerComponentIndex : MonoBehaviour, ICombatCharacter
    {
        /* --------------------------- Component Accessors -------------------------- */

        public Animator Anim
        {
            get => anim;
        }

        public PlayerInput Input
        {
            get => input;
        }

        public PlayerCameraDirector CameraDirector
        {
            get => director;
        }

        public PlayerMotor Motor
        {
            get => motor;
        }

        public PlayerCombatController Combat
        {
            get => combat;
        }

        public PlayerInteractor Interactor
        {
            get => interactor;
        }

        public PlayerStateController State
        {
            get => state;
        }

        public PlayerAttributeContainer Attributes
        {
            get => attributes;
        }

        /* ---------------------------- Combat Properties --------------------------- */

        public HealthAttribute Health
        {
            get => Attributes.Health;
        }

        public MaxHealthAttribute MaxHealth
        {
            get => Attributes.MaxHealth;
        }

        public StaminaAttribute Stamina
        {
            get => Attributes.Stamina;
        }

        public MaxStaminaAttribute MaxStamina
        {
            get => Attributes.MaxStamina;
        }

        public PoiseAttribute Poise
        {
            get => Attributes.Poise;
        }

        public MaxPoiseAttribute MaxPoise
        {
            get => Attributes.MaxPoise;
        }

        public FactionAttribute Faction
        {
            get => Attributes.Faction;
        }

        public StatusAttribute Status
        {
            get => Attributes.Status;
        }

        public bool IsDead
        {
            get => State.IsDead;
        }

        public bool IsAttacking
        {
            get => State.IsAttacking;
        }

        public bool IsStaggered
        {
            get => State.IsStaggered;
        }

        public bool IsBlocking
        {
            get => State.IsBlocking;
        }

        public bool IsDodging
        {
            get => State.IsDodging;
        }

        public bool IsJumping
        {
            get => State.IsJumping;
        }

        /* ------------------------ Inspector-assigned Fields ----------------------- */

        [Header("Component References")]
        [SerializeField] private Animator anim;
        [SerializeField] private PlayerInput input;
        [SerializeField] private PlayerCameraDirector director;
        [SerializeField] private PlayerMotor motor;
        [SerializeField] private PlayerCombatController combat;
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private PlayerStateController state;
        [SerializeField] private PlayerAttributeContainer attributes;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            InitializeInput();
        }

        /* ----------------------------- Initialization ----------------------------- */

        private void InitializeInput()
        {
            input.currentActionMap.Enable();
        }

        /* -------------------------------------------------------------------------- */
    }
}