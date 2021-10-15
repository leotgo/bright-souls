using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    public class AIBehaviourStagger : AIBehaviour
    {

        public override void OnBehaviourStart(AICharacter agent)
        {
            agent.ResetAllTriggers();
            agent.SetMovementControl(AICharacter.AIMovementControlType.Animator);
        }

        public override void OnBehaviourUpdate(AICharacter agent)
        {
        }

        public override void OnBehaviourEnd(AICharacter agent)
        {
        }
    }
}