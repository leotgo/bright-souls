using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPatterns.FiniteStateMachine
{
    [System.Serializable]
    public class StateTransition {

        public IState Source
        {
            get => source;
        }

        public IState Target
        {
            get => target;
        }

        public ITransitionCondition Condition
        {
            get => condition;
        }

        [SerializeReference] private IState source;
        [SerializeReference] private IState target;
        [SerializeReference] private ITransitionCondition condition;
    }
}
