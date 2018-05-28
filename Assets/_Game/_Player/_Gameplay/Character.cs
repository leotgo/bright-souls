using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class Character : MonoBehaviour
{
    [Flags]
    public enum Status
    {
        None = 0x0,
        Staggered = 0x1,
        IFrames = 0x2,
        Unstoppable = 0x4
    }
    public Status _status = Status.None;

    public void AddStatus(Status status)
    {
        _status = _status | status;
    }

    public void RemoveStatus(Status status)
    {
        _status = _status & (~status);
    }

    public bool HasStatus(Status status)
    {
        return (_status & status) != Status.None;
    }

    [Range(1, 1000)]
    public float maxHealth = 100;
    private float _health = 100;
    public float Health {
        get {
            return _health;
        }
        set {
            float lastHealth = _health;
            _health = Mathf.Clamp(value, 0f, maxHealth);
            float diff = _health - lastHealth;
            this.Notify(Message.Combat_HealthChange, diff);
            if (_health <= 0f)
            {
                GetComponent<Animator>().SetTrigger("death");
            }
        }
    }

    private StateMachine[] stateMachines = new StateMachine[0];
    public StateMachine[] StateMachines {
        get {
            if (stateMachines.Length == 0)
                stateMachines = GetComponentsInChildren<StateMachine>();
            return stateMachines;
        }
    }

    public float GetStateTime(States state)
    {
        float time = 0f;
        foreach (var fsm in StateMachines)
            if (fsm.IsInState(state))
                time = fsm.CurrentStateTime > time ? fsm.CurrentStateTime : time;
        return time;
    }

    public void SetState(StateMachineType stateMachine, States state)
    {
        foreach (var fsm in StateMachines)
            if (fsm.type == stateMachine)
                fsm.SetState(state);
    }

    public void Damage(int value)
    {

    }

    public bool IsInAnyState(params States[] states)
    {
        foreach (var s in states)
        {
            foreach (var fsm in StateMachines)
            {
                if (fsm.IsInState(s))
                    return true;
            }
        }
        return false;
    }

    public Vector3 GetDirectionPlanified()
    {
        var dir = transform.forward;
        dir = new Vector3(dir.x, 0f, dir.z);
        return dir.normalized;
    }
}
