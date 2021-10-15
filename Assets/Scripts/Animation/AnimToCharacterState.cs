using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPatterns.FiniteStateMachine;
namespace BrightSouls
{
    public class AnimToCharacterState : StateMachineBehaviour
    {
        /* ------------------------------- Definitions ------------------------------ */

        public enum Condition
        {
            StateEnter,
            StateExit,
            TransitionTo
        }

        /* ------------------------------- Properties ------------------------------- */

        private bool IsNotTransitionCondition
        {
            get { return condition != Condition.TransitionTo; }
        }

        /* ------------------------ Inspector-assigned Fields ----------------------- */

        [SerializeReference] private IState targetState;
        [SerializeField] private Condition condition = Condition.StateEnter;
        [SerializeField] private string nextState = string.Empty;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private IStateMachineOwner stateMachineOwner = null;

        /* ------------------------------ Unity Events ------------------------------ */

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (condition == Condition.StateEnter)
            {
                UpdateState(animator);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (condition == Condition.StateExit)
            {
                UpdateState(animator);
            }
            else if (condition == Condition.TransitionTo)
            {
                if (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(nextState))
                {
                    UpdateState(animator);
                }
            }
        }

        /* --------------------------------- Helpers -------------------------------- */

        private void UpdateState(Animator animator)
        {
            if (stateMachineOwner == null)
            {
                stateMachineOwner = animator.GetComponent<IStateMachineOwner>();
            }
            else
            {
                // TODO Remove setting state machine in animator
                // stateMachineOwner.SetState(targetState);
            }
        }

        /* -------------------------------------------------------------------------- */
    }
}