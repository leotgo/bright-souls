using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransition {

    public States source;
    public States target;

    public delegate bool TransitionCondition();
    public TransitionCondition Condition;

    public StateTransition(States source, States target, TransitionCondition condition)
    {
        this.source = source;
        this.target = target;
        this.Condition = condition;
    }

    public bool HasMetCondition()
    {
        return Condition();
    }

}
