using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPatterns.FiniteStateMachine;

namespace BrightSouls.Gameplay
{
    public enum PlayerStateEnum
    {
        Default,
        Moving,
        Falling,
        Attacking,
        Dodging,
        Comboing,
        ComboEnding,
        Jumping,
        Landing,
        Blocking,
        NotBlocking,
        Dead,
        Seeking,
        Patrolling,
        CombatMovement,
        Staggered,
        Any,
        Sprinting
    }

    public enum AIStates
    {
        Default,
        Any,
        Dead,
        Seeking,
        Patrolling,
        CombatMovement
    }

    // Make implementation of state events optional
    public abstract class PlayerState : IState
    {
        public virtual void OnStateEnter(StateMachineController controller) { }
        public virtual void OnStateUpdate(StateMachineController controller) { }
        public virtual void OnStateExit(StateMachineController controller) { }
    }

    public class PlayerStateStaggered : PlayerState { }
    public class PlayerStateBlocking : PlayerState { }
    public class PlayerStateAttacking : PlayerState { }
    public class PlayerStateComboing : PlayerState { }
    public class PlayerStateComboEnding : PlayerState { }
    public class PlayerStateJumping : PlayerState { }
}