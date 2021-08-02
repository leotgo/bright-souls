using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BrightSouls
{
    [RequireComponent(typeof(Player))]
    public class PlayerMotor : MonoBehaviour
    {
        public enum MotionSourceType
        {
            Motor,
            Animation
        }
        public MotionSourceType MotionSource;

        [SerializeField] private Player player;
        public MoveCommand Move { get; private set; }

        private CharacterController charController;
        [SerializeField] private LayerMask groundLayers;

        private Vector3 gravity = new Vector3(0f, -9.81f, 0f);
        private float gravityMult = 1f;

        private Vector3 movement = Vector3.zero;
        private Vector3 Movement
        {
            get
            {
                return movement;
            }
            set
            {
                movement = value;
                player.Anim.SetFloat("speed_y", movement.y);
            }
        }

        private void Start()
        {
            InitializeComponentReferences();
            InitializeCommands();
            InitializeInput();
        }

        private void Update()
        {
            GravityUpdate();

            if (player.IsInAnyState(States.Dead))
                return;

            charController.Move(Movement * Time.deltaTime);
        }

        private void InitializeComponentReferences()
        {
            charController = GetComponentInChildren<CharacterController>();
        }

        private void InitializeCommands()
        {
            Move = new MoveCommand(this.player);
        }

        private void InitializeInput()
        {
            var move = player.Input.currentActionMap.FindAction("Move");

            move.performed += ctx => Move.Execute(move.ReadValue<Vector2>());
        }

        // Will depend on camera type
        private void MovementUpdate(Vector2 dir)
        {
            Vector2 move = dir;
            if (move.magnitude > 1f)
            {
                move = move.normalized;
            }

            player.Anim.SetFloat("move_speed", move.magnitude);

            float mult = player.IsInAnyState(States.Blocking) ? 0.5f : 1f;

            player.Anim.SetFloat("move_x", move.x * mult);
            player.Anim.SetFloat("move_y", move.y * mult);
        }

        private void GravityUpdate()
        {
            var r = new Ray(transform.position, Vector3.down);
            bool grounded = Physics.SphereCast(r, charController.radius + 0.1f, charController.height / 2f + 0.5f, groundLayers.value);
            player.Anim.SetBool("grounded", grounded);
            // Animator also applies gravity, so when not grounded disable animator physics
            player.Anim.applyRootMotion = grounded;

            if (!grounded)
            {
                Movement += gravity * gravityMult * Time.deltaTime;
            }
            else if (Movement.y < 0f)
            {
                if (Mathf.Abs(Movement.y) > 15f)
                    player.Stamina.Value -= Mathf.CeilToInt(Mathf.Abs(Movement.y) * 3f);
                Movement = new Vector3(Movement.x, 0f, Movement.z);
                charController.Move(new Vector3(0f, -0.5f, 0f));
            }
        }

        public class MoveCommand : PlayerCommand<Vector2>
        {
            public MoveCommand(Player owner) : base(owner) { }

            public override bool IsValid()
            {
                bool isInValidState = !player.IsInAnyState(States.Dodging, States.Stagger, States.Dead);
                return isInValidState;
            }

            public override void Execute(Vector2 dir)
            {
                player.Motor.MovementUpdate(dir);
            }
        }
    }
}