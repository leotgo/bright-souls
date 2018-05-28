using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimToCharacterState : StateMachineBehaviour {

    private Character animatorOwner = null;

    public States targetState = States.Default;
    public StateMachineType stateMachine = StateMachineType.Generic;

    public Condition condition = Condition.StateEnter;
    private bool IsNotTransitionCondition {
        get { return condition != Condition.TransitionTo; }
    }

    [HideIf("IsNotTransitionCondition")]
    public string nextState = string.Empty;

    public enum Condition
    {
        StateEnter,
        StateExit,
        TransitionTo
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (condition == Condition.StateEnter)
            UpdateState(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (condition == Condition.StateExit)
            UpdateState(animator);
        if (condition == Condition.TransitionTo)
        {
            if (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(nextState))
            {
                UpdateState(animator);
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void UpdateState(Animator animator)
    {
        if (animatorOwner == null)
            animatorOwner = animator.GetComponent<Character>();
        if (animatorOwner != null)
            animatorOwner.SetState(stateMachine, targetState);
    }
}
