using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public StateMachineType type = StateMachineType.Generic;
    public string fsmName;

    protected StateTransition[] transitions = new StateTransition[0];

    protected State state;
    protected State[] states;

    private float _stateTime = 0f;
    private string _defaultName;

    public float CurrentStateTime
    {
        get
        {
            return _stateTime;
        }
    }

    public States CurrentState
    {
        get
        {
            return state != null ? state.stateType : States.Default;
        }
    }

    public void SetState(States newState)
    {
        var s = FindStateOfType(newState);
        if (s != null)
        {
            if (state != null)
            {
                state.OnStateExit();
            }

            state = s;
            gameObject.name = _defaultName + " - " + state.name;
            _stateTime = 0f;
            state.OnStateEnter();
        }
        else
        {
            Debug.LogError(this.name + " did not find state of type " + newState);
        }
    }

    public bool IsInState(States state)
    {
        if(this.state != null)
        {
            return this.state.stateType == state;
        }
        else
        {
            return false;
        }
    }

    protected virtual void Initialize()
    {
    }

    protected virtual void Start()
    {
        states = GetComponentsInChildren<State>();
        _defaultName = gameObject.name;
        Initialize();
        SetState(States.Default);
    }

    protected virtual void Update()
    {
        CheckTransitions();
        _stateTime += Time.deltaTime;
        state.OnStateUpdate();
    }

    protected virtual void OnStateTransitionEnter(States source, States target)
    {

    }

    protected virtual void OnStateTransitionExit(States source, States target)
    {

    }

    protected virtual void CheckTransitions()
    {
        foreach (var t in transitions)
        {
            if (t.source == this.state.stateType || t.source == States.Any)
            {

                if(t.HasMetCondition())
                {
                    States source = t.source;
                    States target = t.target;
                    OnStateTransitionEnter(source, target);
                    SetState(target);
                    OnStateTransitionExit(source, target);
                    return;
                }
            }
        }
    }

    private State FindStateOfType(States state)
    {
        foreach (State s in states)
        {
            if (s.stateType == state)
            {
                return s;
            }
        }

        return null;
    }

}

public enum StateMachineType
{
    Generic,
    Movement,
    Combat
}