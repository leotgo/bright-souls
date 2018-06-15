using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviourAttack : AIBehaviour {

    private float turnSpeed = 2.75f;

    public override void BehaviourStart()
    {
        owner.SetMovementControl(AICharacter.MovementControlType.Animator);
        owner.Attack(owner.nextAttack);
    }

    public override void BehaviourUpdate()
    {
        LookAtTarget();
    }

    public override void BehaviourEnd()
    {
        
    }

    private void LookAtTarget()
    {
        owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.LookRotation(owner.GetDirectionToTarget(), Vector3.up), turnSpeed * Time.deltaTime);
    }
}
