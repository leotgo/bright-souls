using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BrightSouls.Player
{
    [RequireComponent(typeof(PlayerComponentIndex))]
    public class PlayerMotor : MonoBehaviour
    {
        /* ---------------------------- Type Definitions ---------------------------- */

        public enum MotionSourceType
        {
            Motor,
            Animation
        }

        /* ------------------------------- Properties ------------------------------- */

        public MoveCommand Move
        {
            get;
            private set;
        }

        // * Speed:
        // *   Only used for vertical movement caused by gravity.
        // *   Ground movement is handled by the Player's Animator.
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

        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [Header("Component Refs")]
        [SerializeField] private PlayerComponentIndex player;
        [SerializeField] private CharacterController charController;

        [Header("Physics Data")]
        [SerializeField] private PlayerPhysicsData physicsData;
        [SerializeField] private WorldPhysicsData  worldPhysicsData;

        /* ----------------------------- Runtime Fields ----------------------------- */

        public MotionSourceType MotionSource;
        private bool grounded = false;
        private Vector3 speed = Vector3.zero;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            InitializeCommands();
            InitializeInput();
        }

        private void Update()
        {
            GravityUpdate();

            if (player.State.IsDead)
            {
                return;
            }

            charController.Move(Speed * Time.deltaTime);
        }

        /* ----------------------------- Initialization ----------------------------- */

        private void InitializeCommands()
        {
            Move = new MoveCommand(this.player);
        }

        private void InitializeInput()
        {
            var move = player.Input.currentActionMap.FindAction("Move");
            move.performed += ctx => Move.Execute(move.ReadValue<Vector2>());
        }

        /* ----------------------------- Public Methods ----------------------------- */

        public void PerformGroundMovement(Vector2 input)
        {
            input = ClampMovementInput(input);
            var moveSpeedMultiplier = GetMovementSpeedMultiplier();
            // Actual transform movement is handled by the animator
            player.Anim.SetFloat("move_speed", input.magnitude);
            player.Anim.SetFloat("move_x", input.x * moveSpeedMultiplier);
            player.Anim.SetFloat("move_y", input.y * moveSpeedMultiplier);
        }

        /* ----------------------------- Private Methods ---------------------------- */

        private void GravityUpdate()
        {
            UpdateGroundedState();
            if (!grounded)
            {
                Speed += worldPhysicsData.Gravity  * Time.deltaTime;
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
            grounded = Physics.SphereCast(ray, charController.radius + 0.1f, charController.height / 2f + 0.5f, physicsData.GroundDetectionLayers.value);
            player.Anim.SetBool("grounded", grounded);
            // Animator also applies gravity, so when not grounded disable animator physics
            player.Anim.applyRootMotion = grounded;
        }

        private void OnHitGround()
        {
            float fallSpeed = Mathf.Abs(Speed.y);
            float fallDamage = CalculateFallDamage(fallSpeed);
            player.Attributes.Health.Value -= fallDamage;
            // Reset the vertical speed when hitting ground
            Speed = new Vector3(Speed.x, 0f, Speed.z);
            // Teleport player vertically to avoid getting stuck in the ground when falling at high speeds
            charController.Move(new Vector3(0f, -0.5f, 0f));
        }

        /* --------------------------------- Helpers -------------------------------- */

        private float GetMovementSpeedMultiplier()
        {
            float moveSpeedMultiplier = 1f;
            bool isBlocking = player.State.IsBlocking;
            if (isBlocking)
            {
                moveSpeedMultiplier *= physicsData.BlockingMoveSpeedMultiplier;
            }
            return moveSpeedMultiplier;
        }

        private float CalculateFallDamage(float fallSpeed)
        {
            if (fallSpeed > physicsData.MinimumFallDamageSpeed)
            {
                return Mathf.CeilToInt(fallSpeed * physicsData.FallDamageMultiplier);
            }
            else
            {
                return 0f;
            }
        }

        private Vector2 ClampMovementInput(Vector2 input)
        {
            if (input.magnitude > 1f)
            {
                return input.normalized;
            }
            else
            {
                if (input.x < 0.1f)
                {
                    input.Set(0f, input.y);
                }
                if (input.y < 0.1f)
                {
                    input.Set(input.x, 0f);
                }
                return input;
            }
        }

        public Vector2 GetDirectionInXZPlane()
        {
            // TODO implement GetDirectionInXZPlane in PlayerMotor
            return Vector2.zero;
        }

        /* -------------------------------------------------------------------------- */
    }
}