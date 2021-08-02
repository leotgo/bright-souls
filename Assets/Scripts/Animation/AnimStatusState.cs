using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls
{

    public class AnimStatusState : StateMachineBehaviour
    {
        private Character animatorOwner;

        public bool activatedStatus = false;

        public CharacterStatus status;

        public float statusStartTime = 0.0f;
        public float statusEndTime   = 1.0f;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animatorOwner = animator.GetComponent<Character>();
            activatedStatus = false;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > statusStartTime && !activatedStatus)
            {
                activatedStatus = true;
                animatorOwner.Attributes.GetAttribute<StatusAttribute>().Value |= status;
            }
            if (stateInfo.normalizedTime > statusEndTime && activatedStatus)
            {
                activatedStatus = false;
                animatorOwner.Attributes.GetAttribute<StatusAttribute>().Value &= ~status;
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            activatedStatus = false;
        }
    }
}