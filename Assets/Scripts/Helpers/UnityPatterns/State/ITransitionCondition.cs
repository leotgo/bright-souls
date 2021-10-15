namespace UnityPatterns.FiniteStateMachine
{
    public interface ITransitionCondition
    {
        bool Validate(StateMachineController controller);
    }
}