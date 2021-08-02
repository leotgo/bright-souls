using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    public class AIBehaviourStagger : AIBehaviour
    {

        public override void BehaviourStart()
        {
            owner.ResetAllTriggers();
            owner.SetMovementControl(AICharacter.AIMovementControlType.Animator);
        }

        public override void BehaviourUpdate()
        {
        }

        public override void BehaviourEnd()
        {
        }
    }
}