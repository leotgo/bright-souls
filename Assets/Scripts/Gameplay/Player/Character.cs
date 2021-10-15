using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using UnityPatterns.FiniteStateMachine;

namespace BrightSouls
{
    public interface ICharacter
    {
        Transform transform { get; }
        bool IsInState<T>() where T : IState;
    }
}
