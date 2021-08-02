using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls
{
    public abstract class Character : MonoBehaviour
    {
        public StateMachine[] StateMachines
        {
            get
            {
                if (stateMachines.Length == 0)
                {
                    stateMachines = GetComponentsInChildren<StateMachine>();
                }
                return stateMachines;
            }
        }

        public abstract AttributesContainer Attributes { get; }

        private StateMachine[] stateMachines = new StateMachine[0];

        public float GetStateTime(States state)
        {
            float time = 0f;
            foreach (var fsm in StateMachines)
            {
                if (fsm.IsInState(state))
                {
                    time = fsm.CurrentStateTime > time ? fsm.CurrentStateTime : time;
                }
            }
            return time;
        }

        public void SetState(StateMachineType stateMachine, States state)
        {
            foreach (var fsm in StateMachines)
            {
                if (fsm.type == stateMachine)
                {
                    fsm.SetState(state);
                }
            }
        }

        public bool IsInAnyState(params States[] states)
        {
            foreach (var s in states)
            {
                foreach (var fsm in StateMachines)
                {
                    if (fsm.IsInState(s))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public Vector3 GetDirectionInXZPlane()
        {
            var dir = transform.forward;
            dir = new Vector3(dir.x, 0f, dir.z);
            return dir.normalized;
        }
    }
}
