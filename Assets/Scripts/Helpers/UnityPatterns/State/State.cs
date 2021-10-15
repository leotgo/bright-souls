using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPatterns.FiniteStateMachine
{
    public interface IState
    {
        void OnStateEnter(StateMachineController fsm);
        void OnStateUpdate(StateMachineController fsm);
        void OnStateExit(StateMachineController fsm);
    }
}
