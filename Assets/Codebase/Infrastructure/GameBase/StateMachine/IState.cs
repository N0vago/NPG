namespace Codebase.Infrastructure.GameBase.StateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}