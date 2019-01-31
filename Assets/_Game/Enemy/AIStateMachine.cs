using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class AIStateMachine : StateMachine, IObserver {

    [NonSerialized] public AICharacter owner;

    protected override void OnStateTransitionEnter(States source, States target)
    {
        owner.ResetAllTriggers();
    }

    protected override void Initialize()
    {
        owner = GetComponentInParent<AICharacter>();

        this.Observe(Message.AI_ReachedWaypoint);
        this.Observe(Message.AI_StartAttack);
        this.Observe(Message.Combat_AttackEnd);
        this.Observe(Message.Combat_Stagger);
        this.Observe(Message.Combat_StaggerEnd);
        this.Observe(Message.Combat_Death);

        transitions = new StateTransition[] {
            new StateTransition(States.CombatMovement, States.Seeking,        () => { return owner.GetDistanceToTarget() > 12f; }),
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

    public void OnNotification(object sender, Message msg , params object[] args)
    {
        var isSenderOwner = (UnityEngine.Object)sender == owner || (UnityEngine.Object)sender == owner.animator;
        if(!isSenderOwner)
            return;

        if(CurrentState == States.Patrolling && msg == Message.AI_ReachedWaypoint)
            SetState(States.Default);
        if(CurrentState == States.Attacking && msg == Message.Combat_AttackEnd)
            SetState(States.CombatMovement);
        if (CurrentState == States.Sprinting && msg == Message.Combat_AttackEnd)
            SetState(States.CombatMovement);
        if(CurrentState == States.Stagger && msg == Message.Combat_StaggerEnd)
            SetState(States.CombatMovement);
        if(msg == Message.Combat_Stagger)
            SetState(States.Stagger);
        if(msg == Message.Combat_Death)
            SetState(States.Dead);
    }
}

