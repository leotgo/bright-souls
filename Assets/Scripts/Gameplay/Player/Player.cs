using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace BrightSouls
{
    /// <summary>
    /// The Player class indexes all components related to the
    /// player-controlled character.
    /// </summary>
    public sealed class Player : Character
    {
        public Animator Anim { get => anim; }
        public PlayerInput Input { get => input; }
        public PlayerCameraDirector CameraDirector { get => director; }
        public PlayerMotor Motor { get => motor; }
        public PlayerCombatController Combat { get => combat; }
        public PlayerInteractor Interactor { get => interactor; }
        public StaggerBehaviour Stagger { get => stagger; }
        public StaminaBehaviour Stamina { get => stamina; }

        public override AttributesContainer Attributes { get => attributes; }

        [Header("Component References")]
        [SerializeField] private Animator anim;
        [SerializeField] private PlayerInput input;
        [SerializeField] private PlayerCameraDirector director;
        [SerializeField] private PlayerMotor motor;
        [SerializeField] private PlayerCombatController combat;
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private StaggerBehaviour stagger;
        [SerializeField] private StaminaBehaviour stamina;

        [Header("Fields")]
        [SerializeField] private AttributesContainer attributes = new AttributesContainer();

        private void Start()
        {
            InitializeInput();
        }

        private void InitializeInput()
        {
            input.currentActionMap.Enable();
        }
    }
}