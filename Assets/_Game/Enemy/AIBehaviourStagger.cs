using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviourStagger : AIBehaviour
{

    public override void BehaviourStart()
    {
        owner.ResetAllTriggers();
        owner.SetMovementControl(AICharacter.MovementControlType.Animator);
    }

    public override void BehaviourUpdate()
    {
    }

    public override void BehaviourEnd()
    {
    }
}
