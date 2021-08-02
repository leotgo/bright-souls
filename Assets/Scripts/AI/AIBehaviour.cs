using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    public abstract class AIBehaviour : MonoBehaviour
    {

        [HideInInspector] public AICharacter owner;
        [HideInInspector] public AIStateMachine fsm;

        public abstract void BehaviourStart();
        public abstract void BehaviourUpdate();
        public abstract void BehaviourEnd();

        private void Start()
        {
            owner = GetComponentInParent<AICharacter>();
            fsm   = GetComponentInParent<AIStateMachine>();
        }
    }
}
