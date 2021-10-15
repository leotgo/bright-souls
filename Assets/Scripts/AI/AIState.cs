using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPatterns.FiniteStateMachine;

namespace BrightSouls.AI
{
    public class AIBehaviourState : IState
    {
        [SerializeReference] private AIBehaviour behaviour;

        public void OnStateEnter(StateMachineController controller)
        {
            var agent = controller.GetComponent<AICharacter>();
            behaviour?.OnBehaviourStart(agent);
        }

        public void OnStateUpdate(StateMachineController controller)
        {
            var agent = controller.GetComponent<AICharacter>();
            behaviour?.OnBehaviourUpdate(agent);
        }

        public void OnStateExit(StateMachineController controller)
        {
            var agent = controller.GetComponent<AICharacter>();
            behaviour?.OnBehaviourEnd(agent);
        }
    }
}