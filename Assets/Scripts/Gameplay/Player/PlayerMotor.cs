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

        public MoveCommand Move
        {
            get;
            private set;
        }

        // Speed is currently only used for vertical movement (caused by gravity)
        private Vector3 Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                player.Anim.SetFloat("speed_y", speed.y);
            }
        }

        // Inspector-assigned values
        [Header("Component Refs")]
        [SerializeField] private Player player;
        [SerializeField] private CharacterController charController;
        [Header("Physics Data")]
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

        // Runtime
        public MotionSourceType MotionSource;
        private bool grounded = false;
        private Vector3 speed = Vector3.zero;

        private void Start()
        {
            InitializeCommands();
            InitializeInput();
        }

        private void Update()
        {
            GravityUpdate();

            if (player.IsInAnyState(States.Dead))
            {
                return;
            }

            charController.Move(Speed * Time.deltaTime);
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

            float blockingSpeedMultiplier = player.IsInAnyState(States.Blocking) ? 0.5f : 1f;

            player.Anim.SetFloat("move_x", move.x * blockingSpeedMultiplier);
            player.Anim.SetFloat("move_y", move.y * blockingSpeedMultiplier);
        }

        private void GravityUpdate()
        {
            UpdateGroundedState();
            if (!grounded)
            {
                Speed += gravity  * Time.deltaTime;
            }
            else
            {
                bool wasFalling = Speed.y < 0f;
                if (wasFalling)
                {
                    OnHitGround();
                }
            }
        }

        private void UpdateGroundedState()
        {
            var ray = new Ray(transform.position, Vector3.down);
            grounded = Physics.SphereCast(ray, charController.radius + 0.1f, charController.height / 2f + 0.5f, groundLayers.value);
            player.Anim.SetBool("grounded", grounded);
            // Animator also applies gravity, so when not grounded disable animator physics
            player.Anim.applyRootMotion = grounded;
        }

        private void OnHitGround()
        {
            float fallSpeed = Mathf.Abs(Speed.y);
            float fallDamage = GetFallDamage(fallSpeed);
            player.Health -= fallDamage;
            // Reset the vertical speed when hitting ground
            Speed = new Vector3(Speed.x, 0f, Speed.z);
            // Teleport player vertically to avoid getting stuck in the ground when falling at high speeds
            charController.Move(new Vector3(0f, -0.5f, 0f));
        }

        private float GetFallDamage(float fallSpeed)
        {
            const float minimumFallDamageSpeed = 15f;
            if (fallSpeed > minimumFallDamageSpeed)
            {
                return Mathf.CeilToInt(fallSpeed * 3f);
            }
            else
            {
                return 0f;
            }
        }

        public class MoveCommand : PlayerCommand<Vector2>
        {
            public MoveCommand(Player owner) : base(owner) { }

            public override bool CanExecute()
            {
                bool canMove = !player.IsInAnyState(States.Dodging, States.Stagger, States.Dead);
                return canMove;
            }

            public override void Execute(Vector2 dir)
            {
                if(CanExecute())
                {
                    player.Motor.MovementUpdate(dir);
                }
            }
        }
    }
}