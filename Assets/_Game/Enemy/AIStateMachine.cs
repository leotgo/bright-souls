using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class AIStateMachine : StateMachine {

    public AICharacter owner;
    public MessageBuffer msgBuffer;

    protected override void OnStateTransitionEnter(States source, States target)
    {
        owner.ResetAllTriggers();
    }

    protected override void Initialize()
    {
        owner = GetComponentInParent<AICharacter>();
        msgBuffer = gameObject.AddComponent<MessageBuffer>();

        msgBuffer.AddSender(owner);
        msgBuffer.AddSender(owner.animator);

        msgBuffer.AddMessage(Message.Combat_Death, BufferClearType.OnConsume);
        msgBuffer.AddMessage(Message.Combat_Stagger, BufferClearType.OnConsume);
        msgBuffer.AddMessage(Message.Combat_StaggerEnd, BufferClearType.OnConsume);
        msgBuffer.AddMessage(Message.AI_ReachedWaypoint, BufferClearType.OnConsume);
        msgBuffer.AddMessage(Message.Combat_AttackEnd, BufferClearType.OnConsume);
        msgBuffer.AddMessage(Message.AI_StartAttack, BufferClearType.NextFrameUpdate);

        transitions = new StateTransition[] {
            new StateTransition(States.Any,            States.Dead,           () => { return msgBuffer.HasReceived(Message.Combat_Death); }),
            new StateTransition(States.Any,            States.Stagger,        () => { return msgBuffer.HasReceived(Message.Combat_Stagger); }),
            new StateTransition(States.Stagger,        States.CombatMovement, () => { return msgBuffer.HasReceived(Message.Combat_StaggerEnd); } ),
            new StateTransition(States.Attacking,      States.CombatMovement, () => { return msgBuffer.HasReceived(Message.Combat_AttackEnd); } ),
            new StateTransition(States.Patrolling,     States.Default,        () => { return msgBuffer.HasReceived(Message.AI_ReachedWaypoint); }),
            new StateTransition(States.CombatMovement, States.Seeking,        () => { return owner.GetDistanceToTarget() > 10f; }),
            new StateTransition(States.CombatMovement, States.Attacking,      () => { return msgBuffer.HasReceived(Message.AI_StartAttack); }),
            new StateTransition(States.Default,        States.Patrolling,     () => { return CurrentStateTime > 2f; }),
            new StateTransition(States.Default,        States.Seeking,        () => { return owner.HasTarget; }),
            new StateTransition(States.Patrolling,     States.Seeking,        () => { return owner.HasTarget; }),
            new StateTransition(States.Seeking,        States.CombatMovement, () => { return owner.GetDistanceToTarget() < 5f; }),
            new StateTransition(States.CombatMovement, States.Default,        () => {
                if(owner.Target.IsInAnyState(States.Dead))
                {
                    owner.Target = null;
                    return true;
                }
                return false;
            })
        };
    }

}

