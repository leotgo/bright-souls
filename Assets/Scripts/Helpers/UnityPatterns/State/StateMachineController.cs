using UnityEngine;

namespace UnityPatterns.FiniteStateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        /* ------------------------------- Properties ------------------------------- */

        public float CurrentStateTime
        {
            get => currentStateTime;
        }

        public IState CurrentState
        {
            get => currentState;
        }

        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private SerializedStateMachine stateMachine;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private string defaultName;
        private IState currentState;
        private float currentStateTime = 0f;

        /* ------------------------------ Unity Events ------------------------------ */
        private void Start()
        {
            defaultName = gameObject.name;
        }

        private void Update()
        {
            CheckTransitions();
            currentStateTime += Time.deltaTime;
            currentState.OnStateUpdate(this);
        }

        /* ----------------------------- Public Methods ----------------------------- */

        public void SetState<T>() where T : IState
        {
            var newState = FindStateOfType<T>();
            SetState(newState);
        }

        public void SetState(IState newState)
        {
            if (newState == null)
            {
                var stateType = newState.GetType();
                Debug.LogError($"SetState<{stateType}> failed: State machine {base.name} does not contain state of type {stateType}.");
                return;
            }
            else
            {
                currentState?.OnStateExit(this);
                currentState = newState;
                gameObject.name = defaultName + " - " + currentState;
                currentStateTime = 0f;
                currentState.OnStateEnter(this);
            }
        }

        public bool IsInState<T>() where T : IState
        {
            return IsInState(typeof(T));
        }

        public bool IsInState(System.Type type)
        {
            if (currentState != null)
            {
                return this.currentState.GetType() == type;
            }
            else
            {
                return false;
            }
        }

        public bool IsInAnyState(params System.Type[] types)
        {
            foreach(var type in types)
            {
                if(IsInState(type))
                {
                    return true;
                }
            }
            return false;
        }

        /* ----------------------------- Private Methods ---------------------------- */

        private void CheckTransitions()
        {
            foreach (var transition in stateMachine.Transitions)
            {
                if (transition.Source == currentState)
                {
                    if (transition.Condition.Validate(this))
                    {
                        IState source = transition.Source;
                        IState target = transition.Target;
                        SetState(target);
                        return;
                    }
                }
            }
        }

        private T FindStateOfType<T>() where T : IState
        {
            foreach (var state in stateMachine.States)
            {
                if (state.GetType() == typeof(T))
                {
                    return (T)state;
                }
            }
            Debug.LogError($"FindStateOfType<{typeof(T)}> failed: Unable to find state of type {typeof(T)} in state machine \"{defaultName}\"");
            return default(T);
        }

        /* -------------------------------------------------------------------------- */
    }
}