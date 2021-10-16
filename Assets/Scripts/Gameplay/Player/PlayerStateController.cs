using UnityEngine;
using UnityPatterns.FiniteStateMachine;

namespace BrightSouls.Gameplay
{
    public sealed class PlayerStateController : MonoBehaviour, IStateMachineOwner
    {
        /* ------------------------------- Properties ------------------------------- */

        public bool IsDead
        {
            get => Fsm.IsInState<PlayerStateDead>();
        }

        public bool IsAttacking
        {
            get => Fsm.IsInAnyState(typeof(PlayerStateAttacking), typeof(PlayerStateComboing), typeof(PlayerStateComboEnding));
        }

        public bool IsStaggered
        {
            get => Fsm.IsInState<PlayerStateStaggered>();
        }

        public bool IsBlocking
        {
            get => Fsm.IsInState<PlayerStateBlocking>();
        }

        public bool IsDodging
        {
            get => Fsm.IsInState<PlayerStateDodging>();
        }

        public bool IsJumping
        {
            get => Fsm.IsInState<PlayerStateJumping>();
        }

        public StateMachineController Fsm
        {
            get => null;
        }
    }
}