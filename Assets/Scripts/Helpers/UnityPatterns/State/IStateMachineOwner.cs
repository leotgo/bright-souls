namespace UnityPatterns.FiniteStateMachine
{
    public interface IStateMachineOwner
    {
        StateMachineController Fsm { get; }
    }
}