using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls
{

    public class AnimStatusState : StateMachineBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private CharacterStatus status;
        [SerializeField] private float statusStartTime = 0.0f;
        [SerializeField] private float statusEndTime   = 1.0f;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private IAttributesContainerOwner attributesContainer;
        private bool activatedStatus;

        /* ---------------------- StateMachineBehaviour Events ---------------------- */

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            attributesContainer = animator.GetComponent<IAttributesContainerOwner>();
            activatedStatus = false;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > statusStartTime && !activatedStatus)
            {
                activatedStatus = true;
                attributesContainer.Attributes.GetAttribute<StatusAttribute>().Value |= status;
            }
            if (stateInfo.normalizedTime > statusEndTime && activatedStatus)
            {
                activatedStatus = false;
                attributesContainer.Attributes.GetAttribute<StatusAttribute>().Value &= ~status;
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            activatedStatus = false;
        }

        /* -------------------------------------------------------------------------- */
    }
}