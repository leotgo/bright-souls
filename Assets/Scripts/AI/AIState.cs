using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    public class AIState : State
    {

        private AIBehaviour behaviour;

        void Start ()
        {
            behaviour = GetComponent<AIBehaviour>();
        }

        public override void OnStateEnter()
        {
            if (behaviour)
                behaviour.BehaviourStart();
        }

        public override void OnStateUpdate()
        {
            if (behaviour)
                behaviour.BehaviourUpdate();
        }

        public override void OnStateExit()
        {
            if (behaviour)
                behaviour.BehaviourEnd();
        }
    }
}