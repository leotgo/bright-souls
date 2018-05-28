using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class AIStateMachine : StateMachine, IObserver {

    public AICharacter owner;

    private Dictionary<Message, bool> _msgBuffer;

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        var senderIsSelf = (Object)sender == owner || (Object)sender == owner.animator;
        if (senderIsSelf)
            _msgBuffer[msg] = true;
    }

    protected override void Initialize()
    {
        owner = GetComponentInParent<AICharacter>();
        _msgBuffer = new Dictionary<Message, bool>();

        TrackMessage(Message.AI_ReachedWaypoint);
        TrackMessage(Message.Combat_Stagger);
        TrackMessage(Message.Combat_StaggerEnd);
        TrackMessage(Message.Combat_Death);
        TrackMessage(Message.Combat_AttackEnd);

        transitions = new StateTransition[] {
            new StateTransition(States.Any,            States.Stagger,        () => { return HasReceivedMessage(Message.Combat_Stagger); }),
            new StateTransition(States.Any,            States.Dead,           () => { return HasReceivedMessage(Message.Combat_Death); }),
            new StateTransition(States.Default,        States.Patrolling,     () => { return CurrentStateTime > 2f; }),
            new StateTransition(States.Default,        States.Seeking,        () => { return owner.HasTarget; }),
            new StateTransition(States.Patrolling,     States.Default,        () => { return HasReceivedMessage(Message.AI_ReachedWaypoint); }),
            new StateTransition(States.Patrolling,     States.Seeking,        () => { return owner.HasTarget; }),
            new StateTransition(States.Seeking,        States.CombatMovement, () => { return owner.GetDistanceToTarget() < 5f; }),
            new StateTransition(States.CombatMovement, States.Seeking,        () => { return owner.GetDistanceToTarget() > 10f; }),
            new StateTransition(States.CombatMovement, States.Default,           () => {
                if(owner.Target.IsInAnyState(States.Dead))
                {
                    owner.Target = null;
                    return true;
                }
                return false;
            }),
            new StateTransition(States.Attacking,      States.CombatMovement, () => { return HasReceivedMessage(Message.Combat_AttackEnd); } ),
            new StateTransition(States.Stagger,        States.CombatMovement, () => { return HasReceivedMessage(Message.Combat_StaggerEnd); } )
        };
    }

    protected void TrackMessage(Message msg)
    {
        this.Observe(msg);
        _msgBuffer.Add(msg, false);
    }

    protected bool HasReceivedMessage(Message msg)
    {
        if (_msgBuffer[msg])
        {
            _msgBuffer[msg] = false;
            return true;
        }
        else
            return false;
    }

}

