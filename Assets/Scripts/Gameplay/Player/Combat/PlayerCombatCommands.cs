using UnityEngine;

namespace BrightSouls
{
    public sealed class PlayerCombatCommands
    {
        public AttackCommand Attack { get; private set; }
        public DefendCommand Defend { get; private set; }
        public DodgeCommand Dodge { get; private set; }

        public PlayerCombatCommands(Player player)
        {
            Attack = new AttackCommand(player);
            Defend = new DefendCommand(player);
            Dodge = new DodgeCommand(player);
        }

        public class AttackCommand : PlayerCommand<int>
        {
            public AttackCommand(Player owner) : base(owner) { }

            private readonly States[] commandBlockerStates =
            {
                States.Dodging,
                States.Jumping,
                States.Blocking,
                States.Stagger,
                States.Dead
            };

            public override bool IsValid()
            {
                bool playerHasStamina = player.Stamina.Value > 0f;
                bool playerIsAbleToAttack = !player.IsInAnyState(commandBlockerStates);
                return playerHasStamina && playerIsAbleToAttack;
            }

            public override void Execute(int attackId)
            {
                player.Motor.MotionSource = PlayerMotor.MotionSourceType.Animation;
                var weapon = player.GetComponentInChildren<Weapon>();
                weapon?.OnAttack(attackId);
            }
        }

        /* --------------------------------- Defend --------------------------------- */

        public class DefendCommand : PlayerCommand<bool>
        {
            public DefendCommand(Player owner) : base(owner) { }

            private readonly States[] commandBlockerStates =
            {
                States.Attacking,
                States.Dodging,
                States.Falling,
                States.Jumping,
                States.Landing,
                States.Comboing,
                States.ComboEnding,
                States.Stagger,
                States.Dead
            };

            public override bool IsValid()
            {
                bool playerIsAbleToDefend = !player.IsInAnyState(commandBlockerStates);
                return playerIsAbleToDefend;
            }

            public override void Execute(bool block)
            {
                //player.Anim.SetBool("block", block);
            }
        }

        /* ---------------------------------- Dodge --------------------------------- */

        public class DodgeCommand : PlayerCommand
        {
            public DodgeCommand(Player player) : base(player) { }

            private readonly States[] commandBlockerStates =
            {
                States.Attacking,
                States.Dodging,
                States.Falling,
                States.Jumping,
                States.Landing,
                States.Comboing,
                States.ComboEnding,
                States.Blocking,
                States.Stagger,
                States.Dead
            };

            public override bool IsValid()
            {
                bool playerHasStamina = player.Stamina.Value > 0f;
                bool playerIsAbleToDodge = !player.IsInAnyState(commandBlockerStates);
                return playerHasStamina && playerIsAbleToDodge;
            }

            public override void Execute()
            {
                Vector3 dodgeDir = player.Combat.ReadDodgeDirectionInCameraSpace();
                // If player did not specify a dodge dir, dodge backwards
                if (dodgeDir.magnitude < 0.2f)
                {
                    if (player.CameraDirector.CurrentCamera.IsThirdPersonCamera)
                    {
                        Quaternion inverseCameraRotation = Quaternion.Inverse(Camera.main.transform.rotation);
                        Vector3 playerBodyForward = new Vector3(player.transform.forward.x, 0f, player.transform.forward.z).normalized;
                        dodgeDir = inverseCameraRotation * playerBodyForward;
                    }
                    else
                    {
                        dodgeDir = Vector3.back;
                    }
                }

                Vector3 camFwd = UnityEngine.Camera.main.transform.forward;
                camFwd = (new Vector3(camFwd.x, 0f, camFwd.z)).normalized;
                if (player.CameraDirector.CurrentCamera.IsThirdPersonCamera)
                {
                    var dir = dodgeDir.magnitude > 0f ? dodgeDir : Vector3.back;
                    var planifiedDir = Vector3.ProjectOnPlane(Camera.main.transform.rotation * dir, Vector3.up).normalized;
                    player.transform.rotation = Quaternion.LookRotation(planifiedDir, Vector3.up);
                    dodgeDir.Set(0f, 0f, 1f);
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

                }

                player.Combat.Events.RaiseOnDodgeExecutedEvent(dodgeDir);
                player.Motor.MotionSource = PlayerMotor.MotionSourceType.Animation;
            }
        }

        /* -------------------------------------------------------------------------- */
    }
}